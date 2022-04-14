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
        TacticalAccounts,
        TelnetChannels,
        Terminal,
        TerminalSettings
    };

    public interface IUiPlatform
    {
        void DisplayForm(AvailableForms form, IFormBacking backingObject);
        void DisplayMainForm(IMainFormBacking backingObject);

        void DisplayModalError(string message, string title);

        void RunUiEventLoop();
    }
}
