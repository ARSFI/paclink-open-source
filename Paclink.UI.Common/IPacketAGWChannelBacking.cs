using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paclink.UI.Common
{
    public interface IPacketAGWChannelBacking : IFormBacking
    {
        string SiteRootDirectory
        {
            get;
        }

        IAGWEngineBacking AgwEngineDialog
        {
            get;
        }

        IEnumerable<string> ChannelNames
        {
            get;
        }

        void UpdateAGWPortInfo(IPacketAgwChannelWindow window);

        bool IsChannelNameAChannel(string name);
        bool IsChannelNameAnAccount(string name);

        void AddChannel(
            string name, int priority, string remoteCallsign, int timeout,
            int agwPacketLength, string agwPort, string agwScript, int agwScriptTimeout,
            bool enabled);

        void UpdateChannel(
            string name, int priority, string remoteCallsign, int timeout,
            int agwPacketLength, string agwPort, string agwScript, int agwScriptTimeout,
            bool enabled);

        void RemoveChannel(string name);

        void GetChannelInfo(
            string name, out int priority, out string remoteCallsign, out int timeout,
            out int agwPacketLength, out int maxOutstanding, out string agwPort, out string agwScript, out int agwScriptTimeout,
            out bool enabled);

        bool ContainsChannel(string name);
    }
}
