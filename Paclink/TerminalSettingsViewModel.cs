using Paclink.UI.Common;

namespace Paclink
{
    internal class TerminalSettingsViewModel : ITerminalSettingsBacking
    {
        public TerminalProperties Properties { get; set; }
        public DialogFormResult DialogResult { get; set; }

        public string CleanSerialPort(string port)
        {
            return Globals.CleanSerialPort(port);
        }

        public void CloseWindow()
        {
            // empty
        }

        public void FormClosed()
        {
            // empty
        }

        public void FormClosing()
        {
            // empty
        }

        public void FormLoaded()
        {
            // empty
        }

        public void FormLoading()
        {
            // empty
        }

        public void RefreshWindow()
        {
            // empty
        }
    }
}
