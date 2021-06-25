using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PointZ.Services.Logger;

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

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                string hostName = Dns.GetHostName();
                await this.udpClient.Client.ConnectAsync(IPAddress.Broadcast, 0, cancellationToken);

                if (this.udpClient.Client.LocalEndPoint is not IPEndPoint localEndPoint)
                    throw new NullReferenceException();

                string localEthernetAddress = localEndPoint.Address.ToString();
                IPEndPoint broadcastAddress = new(IPAddress.Broadcast, 45454);
                string hostNameAndIpAddress = $"{hostName}|{localEthernetAddress}";
                
                _ = this.logger.Log($"Started ({hostNameAndIpAddress})", this);

                while (!cancellationToken.IsCancellationRequested)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(hostNameAndIpAddress);
                    await this.udpClient.SendAsync(bytes, bytes.Length, broadcastAddress);
                    await Task.Delay(1000, cancellationToken);
                }

                _ = this.logger.Log(TaskCancelledMessage, this);
            }
            catch (TaskCanceledException)
            {
                _ = this.logger.Log(TaskCancelledMessage, this);
            }
            catch (Exception e)
            {
                _ = this.logger.Log(e.Message, this);
            }
        }
    }
}