using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace PointZ.Tools
{
    public static class NetTools
    {
        public static async Task<string> GetLocalIpv4Address(CancellationToken token = default)
        {
            UdpClient udpClient = new(0);
            await udpClient.Client.ConnectAsync(IPAddress.Broadcast, 0, token);

            if (udpClient.Client.LocalEndPoint is not IPEndPoint localEndPoint)
                throw new NullReferenceException();

            return localEndPoint.Address.ToString();
        }
    }
}