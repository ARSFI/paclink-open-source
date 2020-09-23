using System.Net;

namespace NLog.Targets.Gelf
{
    public interface ITransportClient
    {
        void Send(byte[] datagram, int bytes, IPEndPoint ipEndPoint);
    }
}
