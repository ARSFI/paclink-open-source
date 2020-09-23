namespace Paclink
{
    public interface IRadio
    {
        // Common properties and methods...
        bool InitializeSerialPort(ref TChannelProperties channel);
        bool SetParameters(ref TChannelProperties channel);
        bool SetDtrControl(bool dtr);
        bool SetRtsControl(bool rts);
        bool SetPTT(bool send);
        void Close();
    }
}
