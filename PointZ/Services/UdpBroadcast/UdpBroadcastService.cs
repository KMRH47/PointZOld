using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PointZ.Services.Logger;
using PointZ.Services.NetTools;

namespace PointZ.Services.UdpBroadcast
{
    public class UdpBroadcastService : IUdpBroadcastService
    {
        private const string TaskCancelledMessage = "The UDP Broadcasting service was forcefully stopped.";
        private readonly UdpClient udpClient;
        private readonly INetToolsService netToolsService;
        private readonly ILogger logger;

        public UdpBroadcastService(UdpClient udpClient, INetToolsService netToolsService, ILogger logger)
        {
            this.udpClient = udpClient;
            this.netToolsService = netToolsService;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                string hostName = Dns.GetHostName();
                const ushort port = 45455;
                string localIpv4Address = await this.netToolsService.GetLocalIpv4Address(cancellationToken);
                string hostNameAndIpAddress = $"{hostName}|{localIpv4Address}";
                await this.logger.Log($"Broadcasting data '{hostNameAndIpAddress}' on port 45455", this);
                IPEndPoint broadcastAddress = new(IPAddress.Broadcast, port);

                while (!cancellationToken.IsCancellationRequested)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(hostNameAndIpAddress);
                    await this.udpClient.SendAsync(bytes, bytes.Length, broadcastAddress);
                    await Task.Delay(1000, cancellationToken);
                }

                await this.logger.Log(TaskCancelledMessage, this);
            }
            catch (TaskCanceledException)
            {
                await this.logger.Log(TaskCancelledMessage, this);
            }
            catch (Exception e)
            {
                await this.logger.Log(e.Message, this);
            }
        }
    }
}