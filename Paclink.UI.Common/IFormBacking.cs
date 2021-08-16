using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paclink.UI.Common
{
    public interface IFormBacking
    {
        void FormLoading();
        void FormLoaded();
        void FormClosing();
        void FormClosed();

        // Events
        void RefreshWindow();
        void CloseWindow();
    }
}
