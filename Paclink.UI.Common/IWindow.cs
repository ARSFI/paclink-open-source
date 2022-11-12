using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paclink.UI.Common
{
    public enum UiDialogResult
    {
        OK,
        Cancel
    }

    public interface IWindowBase
    {
        // Common window actions
        UiDialogResult ShowModal();

        void RefreshWindow();

        void CloseWindow();
    }

    public interface IWindow<IBackingObject> : IWindowBase
    {
        IBackingObject BackingObject { get; }
    }
}
