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

                while (!cancellationToken.IsCancellationRequested)
                {
                    string message = $"[{hostName}] {localEthernetAddress}";
                    Log(message);
                    byte[] bytes = Encoding.UTF8.GetBytes(message);
                    Task<int> task = this.udpClient.SendAsync(bytes, bytes.Length, broadcastAddress);
                    await task;
                    string taskCompletion = task.IsCompletedSuccessfully
                        ? "Broadcast successful!"
                        : "Broadcast failed!";
                    Log(taskCompletion);
                    await Task.Delay(1000, cancellationToken);
                }
                
                Log(TaskCancelledMessage);
            }
            catch (TaskCanceledException)
            {
                Log(TaskCancelledMessage);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        private void Log(string message) => this.logger.Log($"[{DateTime.Now}] UDP Broadcast Service: {message}");
    }
}