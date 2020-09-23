namespace Paclink
{
    public enum LinkStates
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
    }
}
