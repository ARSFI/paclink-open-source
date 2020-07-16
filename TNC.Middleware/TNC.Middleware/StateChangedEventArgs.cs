using System;

namespace TNC.Middleware
{
    public class StateChangedEventArgs : EventArgs
    {
        public ELinkStates State;
        public string Message;
    }
}
