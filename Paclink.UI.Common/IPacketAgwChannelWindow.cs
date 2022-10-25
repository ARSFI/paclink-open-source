using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paclink.UI.Common
{
    public interface IPacketAgwChannelWindow : IWindow<IPacketAGWChannelBacking>
    {
        void AddAGWPortInfo(string AddItem, string Text, bool EnableRetry);

        void ClearItems();
        void SetAgwPort(string port);
        void AddAgwPortItem(string port);
    }
}
