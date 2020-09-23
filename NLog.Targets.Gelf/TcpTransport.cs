using System.Net;
using System.Text;

namespace NLog.Targets.Gelf
{
    class TcpTransport : ITransport
    {
        private readonly ITransportClient _transportClient;

        public string Scheme => "tcp";

        public TcpTransport(ITransportClient transportClient)
        {
            _transportClient = transportClient;
        }

        /// <summary>
        /// Sends a TCP datagram to GrayLog server
        /// </summary>
        /// <param name="ipEndPoint">IP Endpoint of the  of the target GrayLog2 server</param>
        /// <param name="message">Message (in JSON) to log</param>
        public void Send(IPEndPoint ipEndPoint, string message)
        {
            //add null byte to signal end of message
            var msg = message + "\0";
            _transportClient.Send(Encoding.ASCII.GetBytes(msg), msg.Length, ipEndPoint);
        }
    }
}
