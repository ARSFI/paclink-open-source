using System;
using System.Collections.Generic;
using System.Text;

namespace Paclink.UI.Common
{
    public interface IPollingBacking : IFormBacking
    {
        string SiteRootDirectory { get; }

        bool PollOnReceivedMessage { get; }

        bool AutoPoll { get; }

        int AutoPollInterval { get; }

        int MinutesRemaining { get; }

        void LoadParameters();

        void UpdateParameters(bool pollOnReceivedMessage, bool autoPoll, int autoPollInterval, int minutesRemaining);

        void ResetMinutesRemaining();

        void DecrementMinutesRemaining();
    }
}
