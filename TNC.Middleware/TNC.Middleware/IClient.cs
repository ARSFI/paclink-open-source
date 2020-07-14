namespace TNC.Middleware
{
    public interface IClient
    {
        LinkStates State { get; }

        void Poll();
        void Close();
        void Disconnect();
        void Abort();
        bool NormalDisconnect { get; set; }
        void DataToSend(string sData);
        void DataToSend(byte[] binBytes);
        bool Connect(bool blnAutomatic);
        void SendRadioCommand(byte[] bytCommand);
        void SendRadioCommand(string strCommand);
    } 
}
