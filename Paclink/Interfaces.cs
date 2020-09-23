
namespace Paclink
{
    public enum EConnection
    {
        // Enumeration to indicate the origin of a connection...
        Disconnected,
        OutboundConnection,
        InboundConnection
    } // EConnection

    public enum ELinkStates
    {
        // Enumeration for a channel state...
        Undefined,
        Initialized,
        Connecting,
        Callsign,
        Password,
        Connected,
        Disconnected,
        LinkFailed,
        NoSerialPort
    } // EStates

    public enum ELinkDirection
    {
        // Enumeration for a Pactor link direction...
        Disconnected,
        Sending,
        Receiving
    } // ELinkDirection
    
    public interface IClient
    {
        // Common properties and methods...
        ELinkStates State { get; }

        void Poll();
        bool Close();
        void Disconnect();
        void Abort();

        bool NormalDisconnect { get; set; }

        void DataToSend(string sData);
        void DataToSend(byte[] binBytes);
        bool Connect(bool blnAutomatic);
        void SendRadioCommand(byte[] bytCommand);
        void SendRadioCommand(string strCommand);
    } // IClient

    public interface IMessage
    {
        // Common properties and methods...
        string Mime { get; set; }
        string Body { get; set; }

        void AddAttachment(Attachment objAttachment);
    } // IMessage

    public interface IRadio
    {
        // Common properties and methods...
        bool InitializeSerialPort(ref TChannelProperties Channel);
        bool SetParameters(ref TChannelProperties Channel);
        bool SetDtrControl(bool Dtr);
        bool SetRtsControl(bool Rts);
        bool SetPTT(bool Send);
        void Close();
    }
} 