namespace Paclink.UI.Common
{
    public enum AvailableForms
    {
        MainWindow,
        SiteProperties,
        Polling,
        AgwChannels,
        PactorChannels,
        PactorConnect,
        TncChannels,
        AgwEngine,
        ChangePassword,
        CallsignAccounts,
        TacticalAccounts,
        TelnetChannels,
        Terminal,
        TerminalSettings,
        Bearing
    };

    public interface IUiPlatform
    {
        IMainWindow GetMainForm();

        IWindowBase CreateForm(AvailableForms form, IFormBacking backingObject);

        void DisplayForm(AvailableForms form, IFormBacking backingObject);

        void DisplayMainForm(IMainFormBacking backingObject);

        void DisplayModalError(string message, string title);

        void RunUiEventLoop();

        void Yield();
    }
}
