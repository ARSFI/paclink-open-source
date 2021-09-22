using System;
using System.Collections.Generic;
using System.Text;

namespace Paclink.UI.Common
{
    public interface IAGWEngineWindow : IWindow<IAGWEngineBacking>
    {
        enum ButtonStatus { SUCCESS, IN_PROGRESS, FAILED };

        void SetRemoteButtonStatus(bool enabled, ButtonStatus status);
    }
}
