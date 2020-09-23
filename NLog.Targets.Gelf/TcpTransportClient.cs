using System.Net;
using System.Net.Sockets;

namespace NLog.Targets.Gelf
{
    class TcpTransportClient : ITransportClient
    {
        public void Send(byte[] msg, int bytes, IPEndPoint ipEndPoint)
        {
            using (var tcp = new TcpClient(ipEndPoint.Address.ToString(), ipEndPoint.Port))
            {
                // disposition of TcpClient also disposes stream
                var stream = tcp.GetStream();
                stream.Write(msg, 0, msg.Length);
            }
        }
    }
}
