using System;

namespace TNC.Middleware
{
    public class StateChangedEventArgs : EventArgs
    {
        public LinkStates State;
        public string Message;
    }
}
