namespace TNC.Middleware
{
    //TODO: Rename to LinkStates
    public enum ELinkStates
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
