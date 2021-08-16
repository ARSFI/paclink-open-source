using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paclink.UI.Common
{
    public interface IMainFormBacking : IFormBacking
    {
        // Notifies the backing object of its associated window object.
        IMainWindow MainWindow { set; }

        bool UseRMSRelay { get; }
        bool PrimaryThreadExists { get; }
        string ProductVersion { get; }
        bool UpdateComplete { get; }
        bool IsAutoPolling { get; }
        int AutoPollingInterval { get; }
        int AutoPollingMinutesRemaining { get; }
        bool ChannelActive { get; }
        bool HasSimpleTerminal { get; }

        bool HasSMTPStatus { get; }
        bool HasSMTPDisplay { get; }
        bool HasStateDisplay { get; }
        bool HasStatusDisplay { get; }
        bool HasProgressDisplay { get; }
        bool HasRateDisplay { get; }
        bool HasChannelDisplay { get; }

        string Uptime { get; }

        bool IsAGWUsed { get; }

        string SiteRootDirectory { get; }

        void StartChannelAutoconnect();
        void StartChannelOnMainThread(string selectedChannel);
        void PostVersionRecord();

        string GetSMTPStatus();
        string GetSMTPDisplay();
        string GetStateDisplay();
        string GetStatusDisplay();
        string GetProgressDisplay();
        string GetRateDisplay();
        string GetChannelDisplay();

        void UpdateChannelList(Action<string> channelAddAction);
        void ClosePOP3AndSMTPPorts();
        void ShowSiteProperties();
        void OpenPOP3AndSMTPPorts();
        void ShowTacticalAccounts();
        void ShowCallsignAccounts();
        void ShowAGWEngine();
        void ShowPacketAGWChannels();
        void ShowPacketTNCChannels();
        void ShowTelnetChannels();
        void ShowPollingInterval();

        void AbortSelectedModem();

        void ShowPactorChannels();
        void ShowSimpleTerminal();

        // Events
        void UpdateSiteCallsign(string newCallsign);
        void UpdateChannelList();
        void EnableMainWindowInterface();
    }
}
