using System.Collections.Generic;

namespace Paclink.UI.Common
{
    public interface ITelnetChannelsBacking : IFormBacking
    {
        string SiteRootDirectory { get; }
        bool ChannelExists(string channelName);
        void FillChannelCollection();
        int GetChannelPriority(string channelName);
        string GetLastUsedTelnetChannel();
        List<string> GetTelnetChannelNames();
        bool IsAccount(string accountlName);
        bool IsChannel(string channelName);
        bool IsChannelEnabled(string channelName);
        bool IsValidChannelName(string channelName);
        void RemoveChannel(string channelName);
        void SaveCurrentTelnetChannel(string channelName);
        void AddChannel(string channelName, ChannelMode channelType, int priority, bool enabled, bool autoForward, string remoteCallsign);
        void UpdateChannel(string channelName, ChannelMode channelType, int priority, bool enabled, bool autoForward, string remoteCallsign);
    }
}
