namespace Paclink.UI.Common
{
    public interface ITerminalSettingsBacking : IFormBacking
    {
        TerminalProperties Properties { get; set; }
        string CleanSerialPort(string port);
        DialogFormResult DialogResult { get; set; }
    }
}
