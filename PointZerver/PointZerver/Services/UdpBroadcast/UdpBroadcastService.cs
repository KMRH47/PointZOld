using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PointZerver.Services.Logger;

namespace PointZerver.Services.UdpBroadcast
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

        public async Task StartAsync(CancellationToken token, ushort port, int delayMs = 1000)
        {
            try
            {
                this.udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
                EndPoint localEndPoint = this.udpClient.Client.LocalEndPoint;
                IPEndPoint localIpEndPoint = (IPEndPoint)localEndPoint;
                EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, port);
                await this.logger.Log($"Broadcasting from '{localIpEndPoint.Address}'.", this);
                string hostName = Dns.GetHostName();

                while (true)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(hostName);
                    await this.udpClient.Client.SendToAsync(bytes, SocketFlags.None, remoteEndPoint);
                    await Task.Delay(delayMs, token);
                }
            }
            catch (TaskCanceledException)
            {
                await this.logger.Log(TaskCancelledMessage, this);
            }
            catch (OperationCanceledException)
            {
                await this.logger.Log(TaskCancelledMessage, this);
            }
            catch (Exception e)
            {
                await this.logger.Log($"{e.Message}\n{e.StackTrace}", this);
            }
        }
    }
}