using System;
using System.Collections.Generic;
using System.Text;

namespace Paclink.UI.Common
{
    public enum AvailableForms
    {
        MainWindow,
        SiteProperties,
        Polling,
        AgwEngine,
        ChangePassword,
        CallsignAccounts,
        TacticalAccounts
    };

    public interface IUiPlatform
    {
        void DisplayForm(AvailableForms form, IFormBacking backingObject);
        void DisplayMainForm(IMainFormBacking backingObject);

        void DisplayModalError(string message, string title);

        void RunUiEventLoop();
    }
}
