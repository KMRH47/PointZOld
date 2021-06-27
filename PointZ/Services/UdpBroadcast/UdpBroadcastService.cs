using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PointZ.Services.Logger;
using PointZ.Tools;

namespace PointZ.Services.UdpBroadcast
{
    public class UdpBroadcastService : IUdpBroadcastService
    {
        private const string TaskCancelledMessage = "The UDP Broadcasting service was forcefully stopped.";
        private readonly UdpClient udpClient;
        private readonly ILogger logger;

        public UdpBroadcastService(UdpClient udpClient, ILogger logger)
        {
            this.udpClient = udpClient;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken token)
        {
            try
            {
                string hostName = Dns.GetHostName();
                const ushort port = 45455;
                string localIpv4Address = await NetTools.GetLocalIpv4Address(token);
                string hostNameAndIpAddress = $"{hostName}|{localIpv4Address}";
                await this.logger.Log($"Broadcasting '{hostNameAndIpAddress}' on port 45455", this);
                IPEndPoint broadcastAddress = new(IPAddress.Broadcast, port);

                while (!token.IsCancellationRequested)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(hostNameAndIpAddress);
                    await this.udpClient.SendAsync(bytes, bytes.Length, broadcastAddress);
                    await Task.Delay(1000, token);
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