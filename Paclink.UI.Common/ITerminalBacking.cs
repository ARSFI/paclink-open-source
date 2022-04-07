namespace Paclink.UI.Common
{
    public interface ITerminalBacking : IFormBacking
    {
        int GetComCloseTime { get; }
        bool EditTerminalProperties(TerminalProperties terminalProperties);
        TerminalProperties LoadTerminalProperties();
        void SaveTerminalProperties(TerminalProperties terminalProperties);
    }
}
