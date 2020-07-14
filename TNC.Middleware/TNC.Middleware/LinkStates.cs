namespace TNC.Middleware
{
    public enum LinkStates
    {
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
