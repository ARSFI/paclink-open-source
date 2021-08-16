using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paclink.UI.Common
{
    public interface IWindow<IBackingObject>
    {
        IBackingObject BackingObject { get; }

        // Common window actions
        void RefreshWindow();
        void CloseWindow();
    }
}
