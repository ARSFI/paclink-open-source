using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace SyslogLib
{
    public class Syslog 
    {
        /// <summary>
        /// IP Address or Host name of your Syslog server
        /// </summary>
        public string SyslogServer { get; set; }

        /// <summary>
        /// Port number syslog is running on (usually 514)
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Name of the application that will show up in the syslog log
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Machine name hosting syslog
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// The syslog server protocol (tcp/udp) 
        /// </summary>
        public ProtocolType Protocol { get; set; }

        /// <summary>
        /// If set, split message by newlines and send as separate messages
        /// </summary>
        public bool SplitNewlines { get; set; }

        private string strIPaddress;
        private string strLastMessage;
        private DateTime dttLastMessageTime;
        /// <summary>
        /// Initializes a new instance of the Syslog class
        /// </summary>
        public Syslog()
        {
            // Sensible defaults...
            SyslogServer = "graylog.winlink.org";
            strIPaddress = "";
            Port = 514;
            Sender = Assembly.GetCallingAssembly().GetName().Name;
            Protocol = ProtocolType.Udp;
            MachineName = Dns.GetHostName();
            SplitNewlines = true;
            strLastMessage = "";
            dttLastMessageTime = DateTime.MinValue;
        }

        /*--------------------------------------------------------------------------------------------
         * Write a standard log entry for a Winlink program.
         */
        public string WriteLogEntry(string strCallsign, string strProgram, string strVersion, string strMessage, SyslogSeverity severity = SyslogSeverity.Informational)
        {
            string msg = strCallsign + "|" + strProgram + "|" + strVersion + "|" + strMessage;
            return Write(msg, severity);
        }

        /*-------------------------------------------------------------------------------------------
         * Write a message to the system log.
         */
        public string Write(string msg, SyslogSeverity severity = SyslogSeverity.Informational, SyslogFacility facility = SyslogFacility.Local6)
        {
            /* Prevent sending the same message repeatedly */
            if (msg == strLastMessage && (DateTime.UtcNow - dttLastMessageTime).TotalMinutes < 60)
            {
                return "";
            }
            strLastMessage = msg;
            dttLastMessageTime = DateTime.UtcNow;
            string strError = "";
            var formattedMessageLines = GetFormattedMessageLines(msg);
            foreach (var formattedMessageLine in formattedMessageLines)
            {
                var message = BuildSyslogMessage(severity, facility, DateTime.UtcNow, Sender, formattedMessageLine);
                strError = SendMessage(SyslogServer, Port, message, Protocol);
                if (strError != "") break;
            }
            return strError;
        }

        private IEnumerable<string> GetFormattedMessageLines(string msg)
        {
            return SplitNewlines ? msg.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries) : new[] { msg };
        }

        /// <summary>
        /// Performs the actual network part of sending a message
        /// </summary>
        /// <param name="logServer">The syslog server's host name or IP address</param>
        /// <param name="port">The UDP port that syslog is running on</param>
        /// <param name="msg">The syslog formatted message ready to transmit</param>
        /// <param name="protocol">The syslog server protocol (tcp/udp)</param>
        private string SendMessage(string logServer, int port, byte[] msg, ProtocolType protocol)
        {
            if (strIPaddress == "")
            {
                /* Convert server domain name to IP address */
                try
                {
                    var logServerIp = Dns.GetHostAddresses(logServer).FirstOrDefault();
                    if (logServerIp == null)
                    {
                        return "Invlid log server IP";
                    }
                    strIPaddress = logServerIp.ToString();
                }
                catch (Exception ex)
                {
                    return "No Internet or unable to get IP address of log server: " + ex.Message;
                }
            }
            switch (protocol)
            {
                case ProtocolType.Udp:
                    try
                    {
                        using (var udp = new UdpClient(strIPaddress, port))
                        {
                            udp.Send(msg, msg.Length);
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                    break;
                case ProtocolType.Tcp:
                    try
                    {
                        using (var tcp = new TcpClient(strIPaddress, port))
                        {
                            // disposition of tcp also disposes stream
                            var stream = tcp.GetStream();
                            stream.Write(msg, 0, msg.Length);
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                    break;
                default:
                    return "Protocol " + protocol + " is not supported.";
            }
            return "";
        }

        /// <summary>
        /// Builds a syslog-compatible message using the information we have available. 
        /// </summary>
        /// <param name="facility">Syslog Facility to transmit message from</param>
        /// <param name="priority">Syslog severity level</param>
        /// <param name="time">Time stamp for log message</param>
        /// <param name="sender">Name of the subsystem sending the message</param>
        /// <param name="body">Message text</param>
        /// <returns>Byte array containing formatted syslog message</returns>
        private byte[] BuildSyslogMessage( SyslogSeverity priority, SyslogFacility facility, DateTime time, string sender, string body)
        {
            // Get sender machine name
            var machine = MachineName + " ";

            // Calculate PRI field
            var calculatedPriority = (int)facility * 8 + (int)priority;
            var pri = "<" + calculatedPriority.ToString(CultureInfo.InvariantCulture) + ">";

            var timeToString = time.ToString("MMM dd HH:mm:ss ");
            sender = sender + ": ";

            string[] strParams = { pri, timeToString, machine, sender, body, Environment.NewLine };
            return Encoding.ASCII.GetBytes(string.Concat(strParams));
        }
    }
}
