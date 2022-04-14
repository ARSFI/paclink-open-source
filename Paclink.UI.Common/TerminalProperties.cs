namespace Paclink.UI.Common
{
    public class TerminalProperties
    {
        public string Port { get; set; }
        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public int StopBits { get; set; }
        public int Parity { get; set; }
        public int Handshake { get; set; }
        public int WriteTimeout { get; set; }
        public bool RTSEnable { get; set; }
        public bool DTREnable { get; set; }
        public bool LocalEcho { get; set; }
        public bool WordWrap { get; set; }
        public BufferType BufferType { get; set; }       
    }
}
