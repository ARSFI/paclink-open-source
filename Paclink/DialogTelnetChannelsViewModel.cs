using Paclink.UI.Common;
using System.Collections.Generic;

namespace Paclink
{
    internal class DialogTelnetChannelsViewModel : ITelnetChannelsBacking
    {
        public string SiteRootDirectory => Globals.SiteRootDirectory;

        public void AddChannel(string channelName, ChannelMode channelType, int priority, bool enabled, bool autoForward, string remoteCallsign)
        {
            var channel = new ChannelProperties();
            channel.ChannelName = channelName;
            channel.ChannelType = channelType;
            channel.Priority = priority;
            channel.Enabled = enabled;
            channel.EnableAutoforward = autoForward;
            channel.RemoteCallsign = remoteCallsign;
            Channels.AddChannel(ref channel);
        }

        public bool ChannelExists(string channelName)
        {
            return Channels.Entries.ContainsKey(channelName);
        }

        public void FillChannelCollection()
        {
            Channels.FillChannelCollection();
        }

        public int GetChannelPriority(string channelName)
        {
            var channel = (ChannelProperties)Channels.Entries[channelName];
            return channel.Priority;
        }

        public string GetLastUsedTelnetChannel()
        {
            return Globals.Settings.Get("Properties", "Last Telnet Channel", "");
        }

        public List<string> GetTelnetChannelNames()
        {
            var channelNames = new List<string>();
            foreach (ChannelProperties channel in Channels.Entries.Values)
            {
                if (channel.ChannelType == ChannelMode.Telnet)
                {
                    channelNames.Add(channel.ChannelName);
                }
            }
            return channelNames;
        }

        public bool IsAccount(string accountlName)
        {
            return Channels.IsAccount(accountlName);
        }

        public bool IsChannel(string channelName)
        {
            return Channels.IsChannel(channelName);
        }

        public bool IsChannelEnabled(string channelName)
        {
            var channel = (ChannelProperties)Channels.Entries[channelName];
            return channel.Enabled;
        }

        public bool IsValidChannelName(string channelName)
        {
            return Globals.IsValidFileName(channelName);
        }

        public void RemoveChannel(string channelName)
        {
            Channels.RemoveChannel(channelName);
        }

        public void SaveCurrentTelnetChannel(string channelName)
        {
            Globals.Settings.Save("Properties", "Last Telnet Channel", channelName);
        }

        public void UpdateChannel(string channelName, ChannelMode channelType, int priority, bool enabled, bool autoForward, string remoteCallsign)
        {
            var channel = new ChannelProperties();
            channel.ChannelName = channelName;
            channel.ChannelType = channelType;
            channel.Priority = priority;
            channel.Enabled = enabled;
            channel.EnableAutoforward = autoForward;
            channel.RemoteCallsign = remoteCallsign;
            Channels.UpdateChannel(ref channel);
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
