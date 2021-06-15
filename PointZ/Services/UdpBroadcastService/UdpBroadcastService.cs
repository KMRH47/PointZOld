using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PointZ.Services.LoggerService;

namespace PointZ.Services.UdpBroadcastService
{
    public class UdpBroadcastService : IUdpBroadcastService
    {
        private readonly ILogger logger;
        private readonly UdpClient udpClient;

        public UdpBroadcastService(UdpClient udpClient, ILogger logger)
        {
            this.udpClient = udpClient;
            this.logger = logger;
        }

        public async Task StartAsync()
        {
            try
            {
                this.logger.Log("[UDP Broadcaster] Listening...");
                
                while (true)
                {
                    UdpReceiveResult result = await this.udpClient.ReceiveAsync();
                    _ = HandleResultAsync(result);
                }
            }
            catch (Exception e)
            {
                this.logger.Log(e.Message);
            }
        }

        private Task HandleResultAsync(UdpReceiveResult result)
        {
            string message = Encoding.UTF8.GetString(result.Buffer);
            this.logger.Log($"[{result.RemoteEndPoint}]: {message}");
            return Task.CompletedTask;
        }
    }
}