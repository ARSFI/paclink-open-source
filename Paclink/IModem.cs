using Paclink;

public interface IModem
{
    // Common properties and methods...
    LinkStates State { get; }

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
}