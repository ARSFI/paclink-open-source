using Newtonsoft.Json;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace NLog.Targets.Gelf
{
    [Target("Gelf")]
    public class GelfTarget : TargetWithLayout
    {
        private readonly Lazy<IPEndPoint> _lazyIpEndpoint;
        private readonly Lazy<ITransport> _lazyITransport;

        [Required]
        public Uri Endpoint { get; set; }

        [ArrayParameter(typeof(GelfParameterInfo), "parameter")]
        public IList<GelfParameterInfo> Parameters { get; private set; }

        public string Facility { get; set; }
        public bool SendLastFormatParameter { get; set; }

        public IConverter Converter { get; private set; }
        public IEnumerable<ITransport> Transports { get; private set; }
        public DnsBase Dns { get; private set; }

        public GelfTarget() : this(new ITransport[] { new UdpTransport(new UdpTransportClient()), new TcpTransport(new TcpTransportClient()) },
            new GelfConverter(),
            new DnsWrapper())
        {
        }

        public GelfTarget(IEnumerable<ITransport> transports, IConverter converter, DnsBase dns)
        {
            Dns = dns;
            Transports = transports;
            Converter = converter;
            Parameters = new List<GelfParameterInfo>();
            _lazyIpEndpoint = new Lazy<IPEndPoint>(() =>
            {
                var addresses = Dns.GetHostAddresses(Endpoint.Host);
                var ip = addresses.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
                if (ip == null) return null; 
                return new IPEndPoint(ip, Endpoint.Port);
            });
            _lazyITransport = new Lazy<ITransport>(() =>
            {
                return Transports.Single(x => x.Scheme.ToUpper() == Endpoint.Scheme.ToUpper());
            });
        }

        public void WriteLogEventInfo(LogEventInfo logEvent)
        {
            Write(logEvent);
        }

        protected override void Write(LogEventInfo logEvent)
        {
            foreach (var par in Parameters)
            {
                if (!logEvent.Properties.ContainsKey(par.Name))
                {
                    string stringValue = par.Layout.Render(logEvent);

                    logEvent.Properties.Add(par.Name, stringValue);
                }
            }

            if (SendLastFormatParameter && logEvent.Parameters != null && logEvent.Parameters.Any())
            {
                //PromoteObjectPropertiesMarker used as property name to indicate that the value should be treated as a object 
                //whose proeprties should be mapped to additional fields in graylog 
                logEvent.Properties.Add(ConverterConstants.PromoteObjectPropertiesMarker, logEvent.Parameters.Last());
            }

            var jsonObject = Converter.GetGelfJson(logEvent, Facility, Layout);
            if (jsonObject == null) return;
            _lazyITransport.Value
                .Send(_lazyIpEndpoint.Value, jsonObject.ToString(Formatting.None, null));
        }
    }
}
