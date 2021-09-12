using Paclink.UI.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Paclink
{
    public class DialogPollingViewModel : IPollingBacking
    {
        public string SiteRootDirectory => Globals.SiteRootDirectory;

        public bool PollOnReceivedMessage { get; private set; }

        public bool AutoPoll { get; private set; }

        public int AutoPollInterval { get; private set; }

        public int MinutesRemaining { get; private set; }

        public DialogPollingViewModel()
        {
            LoadParameters();
        }

        public void LoadParameters()
        {
            AutoPoll = Globals.Settings.Get(Globals.strProductName, "Auto Poll", false);
            PollOnReceivedMessage = Globals.Settings.Get(Globals.strProductName, "Poll on Received Message", false);
            AutoPollInterval = Globals.Settings.Get(Globals.strProductName, "Auto Poll Interval", 60);
            MinutesRemaining = AutoPollInterval;
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

        public void UpdateParameters(bool pollOnReceivedMessage, bool autoPoll, int autoPollInterval, int minutesRemaining)
        {
            AutoPoll = autoPoll;
            PollOnReceivedMessage = pollOnReceivedMessage;
            AutoPollInterval = autoPollInterval;
            MinutesRemaining = minutesRemaining;

            Globals.Settings.Save(Globals.strProductName, "Auto Poll", AutoPoll);
            Globals.Settings.Save(Globals.strProductName, "Poll on Received Message", PollOnReceivedMessage);
            Globals.Settings.Save(Globals.strProductName, "Auto Poll Interval", AutoPollInterval);
        }

        public void ResetMinutesRemaining()
        {
            MinutesRemaining = AutoPollInterval;
        }

        public void DecrementMinutesRemaining()
        {
            MinutesRemaining--;
        }
    }
}
