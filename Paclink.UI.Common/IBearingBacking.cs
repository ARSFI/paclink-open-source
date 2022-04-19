namespace Paclink.UI.Common
{
    public interface IBearingBacking : IFormBacking
    {
        bool EndBearingDisplay { get; set; }
        string ConnectedCallsign { get; }
    }
}
