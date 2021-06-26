using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace PointZ.Services.NetTools
{
    public class NetToolsService : INetToolsService
    {
        private readonly UdpClient udpClient;

        public NetToolsService(UdpClient udpClient) => this.udpClient = udpClient;

        public async Task<string> GetLocalIpv4Address(CancellationToken cancellationToken)
        {
            if (!this.udpClient.Client.Connected)
                await Connect(cancellationToken);
            if (this.udpClient.Client.LocalEndPoint is not IPEndPoint localEndPoint)
                throw new NullReferenceException();

            return localEndPoint.Address.ToString();
        }

        private async Task Connect(CancellationToken cancellationToken) =>
            await this.udpClient.Client.ConnectAsync(IPAddress.Broadcast, 0, cancellationToken);
    }
}