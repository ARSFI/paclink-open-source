namespace Paclink.UI.Common
{
    public interface ITerminalBacking : IFormBacking
    {
        int GetComCloseTime { get; }
        bool TerminalIsActive { get; set; }
        bool EditTerminalProperties(TerminalProperties terminalProperties);
        TerminalProperties LoadTerminalProperties();
        void SaveTerminalProperties(TerminalProperties terminalProperties);
    }
}
