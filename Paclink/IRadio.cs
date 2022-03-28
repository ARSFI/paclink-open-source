namespace Paclink
{
    public interface IRadio
    {
        // Common properties and methods...
        bool InitializeSerialPort(ref ChannelProperties channel);
        bool SetParameters(ref ChannelProperties channel);
        bool SetDtrControl(bool dtr);
        bool SetRtsControl(bool rts);
        bool SetPTT(bool send);
        void Close();
    }
}
