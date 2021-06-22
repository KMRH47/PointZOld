using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PointZ.Services.Logger;

namespace PointZ.Services.UdpBroadcast
{
    public class UdpBroadcastService : IUdpBroadcastService
    {
        private readonly UdpClient udpClient;
        private readonly ILogger logger;

        public UdpBroadcastService(UdpClient udpClient, ILogger logger)
        {
            this.udpClient = udpClient;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                string hostName = Dns.GetHostName();
                await this.udpClient.Client.ConnectAsync(IPAddress.Broadcast, 65530, cancellationToken);

                if (this.udpClient.Client.LocalEndPoint is not IPEndPoint localEndPoint)
                    throw new NullReferenceException();

                string localEthernetAddress = localEndPoint.Address.ToString();
                IPEndPoint broadcastAddress = new(IPAddress.Broadcast, 45454);

                while (!cancellationToken.IsCancellationRequested)
                {
                    this.logger.Log($"Sending...");
                    string message = $"[{hostName}] {localEthernetAddress}";
                    byte[] bytes = Encoding.UTF8.GetBytes(message);
                    await this.udpClient.SendAsync(bytes, bytes.Length, broadcastAddress);
                    this.logger.Log($"\"{message}\" sent!");
                    await Task.Delay(1000, cancellationToken);
                }

                this.logger.Log("UDP Broadcasting service stopped!");
            }
            catch (TaskCanceledException e)
            {
                this.logger.Log($"The UDP Broadcasting service was forcefully stopped.");
            }
            catch (Exception e)
            {
                this.logger.Log($"{e.Message}.");
            }
        }
    }
}