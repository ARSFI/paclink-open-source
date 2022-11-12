using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paclink.UI.Common
{
    public interface IPactorConnectBacking : IFormBacking
    {
        bool TNCBusyHold
        {
            get;
        }

        string SiteRootDirectory
        {
            get;
        }

        bool PactorDialogResume
        {
            get;
            set;
        }

        bool PactorDialogResuming
        {
            get;
            set;
        }

        string TNCType
        {
            get;
        }

        int AudioToneCenter
        {
            get;
        }

        int WindowTop
        {
            get;
            set;
        }

        int WindowLeft
        {
            get;
            set;
        }

        string ChannelName
        {
            get;
        }

        string ServiceCodes
        {
            get;
        }

        string RemoteCallsign
        {
            get;
        }

        string CenterFrequency
        {
            get;
        }

        IEnumerable<string> ChannelNames
        {
            get;
        }

        bool CanRadioControl
        {
            get;
        }

        bool IsValidFrequency
        {
            get;
        }

        IEnumerable<string> GetFrequencies(string callsign, string tncName);

        bool IsValidCallsign(string callsign);

        string GetCenterFreq(string channelInfo);

        bool IsValidFreq(string centerFreq, out int intFreqHz);

        void PollModem();

        void UpdateChannel(string remoteCallsign, string centerFrequency);

        void SetRadioControlInfo(string centerFreq);

        bool RefreshRadioControlInfo();
    }
}
