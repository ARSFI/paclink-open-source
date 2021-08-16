using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paclink.UI.Common
{
    public interface IMainWindow : IWindow<IMainFormBacking>
    {
        // Show "waiting" UI (e.g. cursors on Windows).
        void EnableWaitDisplay();
        void DisableWaitDisplay();

        // Update title in callsign
        void UpdateSiteCallsign(string callsign);

        // Update channel list
        void UpdateChannelList();

        // Enable main menu
        void EnableMainWindowInterface();
    }
}
