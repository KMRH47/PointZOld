using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using PointZClient.Extensions;
using PointZClient.Services.Logger;
using PointZClient.Tools;

namespace PointZClient.Services.UdpListener
{
    public class UdpListenerService : IUdpListenerService
    {
        private readonly UdpClient udpClient;
        private readonly ILogger logger;

        public UdpListenerService(UdpClient udpClient, ILogger logger)
        {
            this.udpClient = udpClient;
            this.logger = logger;
        }

        public async Task StartAsync()
        {
            try
            {
                string localIpv4Address = await NetworkTools.GetLocalIpv4Address();
                int port = this.udpClient.GetBoundPort();
                string hostNameAndIpAddress = $"{localIpv4Address}:{port}";
                await this.logger.Log($"Listening on '{hostNameAndIpAddress}'.", this);

                while (true)
                {
                    UdpReceiveResult result = await this.udpClient.ReceiveAsync();
                }
            }
            catch (SocketException e)
            {
                await this.logger.Log($"[{nameof(SocketException)}] {e.Message}", this);
            }
            catch (Exception e)
            {
                await this.logger.Log($"[{nameof(Exception)}] {e.Message}", this);
            }
        }
    }
}