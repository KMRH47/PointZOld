using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PointZerver.Services.Logger;
using PointZerver.Tools;

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

        public async Task StartAsync(CancellationToken token, ushort port = 45455, int delayMs = 1000)
        {
            try
            {
                string hostName = Dns.GetHostName();
                TimeSpan delay = TimeSpan.FromMilliseconds(delayMs);
                await this.logger.Log(
                    $"Broadcasting '{hostName}' on port {port} (delay: {delay.Seconds}s {delay.Milliseconds}ms).",
                    this);
                await this.udpClient.Client.ConnectAsync(IPAddress.Broadcast, port, token);

                while (true)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(hostName);
                    await this.udpClient.Client.SendAsync(bytes, SocketFlags.None, token);
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