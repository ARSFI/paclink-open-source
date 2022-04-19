using Paclink.UI.Common;

namespace Paclink
{
    public class BearingViewModel : IBearingBacking
    {
        public bool EndBearingDisplay
        {
            get { return Globals.blnEndBearingDisplay; }
            set { Globals.blnEndBearingDisplay = value; }
        }

        public string ConnectedCallsign
        {
            get { return Globals.strConnectedCallsign; }
        }

        public void CloseWindow()
        {
            // empty
        }

        public void FormClosed()
        {
            // empty
        }

        public void FormClosing()
        {
            // empty
        }

        public void FormLoaded()
        {
            // empty
        }

        public void FormLoading()
        {
            // empty
        }

        public void RefreshWindow()
        {
            // empty
        }
    }
}
