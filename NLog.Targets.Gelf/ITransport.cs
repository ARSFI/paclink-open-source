using System.Net;
namespace NLog.Targets.Gelf
{
    public interface ITransport
    {
        string Scheme { get; }
        void Send(IPEndPoint target, string message);
    }
}
