using System;

namespace TNC.Middleware
{
    public interface IModem
    {
        event EventHandler StateChanged;

        LinkStates State { get; }
        bool NormalDisconnect { get; set; } //TODO: Only assigned - never used - remove

        void Abort();
        void Close();
        bool Connect(bool automatic);
        void DataToSend(string data);
        void DataToSend(byte[] bytes);
        void Disconnect();
        void Poll();

        //for radio control (when available)
        void SendRadioCommand(byte[] command);
        void SendRadioCommand(string command);
    }

    //public class SomeExampleModem : IModem
    //{
    //    public event EventHandler StateChanged;
    //
    //    protected virtual void OnStateChanged(StateChangedEventArgs e)
    //    {
    //        StateChanged?.Invoke(this, e);
    //    }
    //
    //    void SomeMethod()
    //    {
    //        // Do something here before the event…  
    //
    //        OnStateChanged(new StateChangedEventArgs { State = LinkStates.Connected, Message = "We're connected now." });
    //
    //        // or do something here after the event.
    //    }
    //
    //    ... other interface method implementations
    //}
    //
    //then, a SomeExampleModem instance defines an event handler
    //that will be called for each state change action
    // sem.StateChanged += StateChangedHandler;

}
