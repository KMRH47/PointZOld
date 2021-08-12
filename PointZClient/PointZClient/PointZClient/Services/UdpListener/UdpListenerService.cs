using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PointZClient.Models.Server;
using PointZClient.Services.Logger;

namespace PointZClient.Services.UdpListener
{
    public class UdpListenerService : IUdpListenerService
    {
        private readonly UdpClient udpClient;
        private readonly ILogger logger;
        private Action<ServerData> onServerDataReceived;

        public UdpListenerService(UdpClient udpClient, ILogger logger)
        {
            this.udpClient = udpClient;
            this.logger = logger;
        }

        public bool Running { get; private set; }

        public async Task StartAsync(Action<ServerData> onServerDataReceived)
        {
            try
            {
                this.onServerDataReceived = onServerDataReceived;
                Running = true;
                IPEndPoint endPoint = new(IPAddress.Any, 45455);
                this.udpClient.Client.Bind(endPoint);

                while (true)
                {
                    
                    UdpReceiveResult result = await this.udpClient.ReceiveAsync();
                    _ = HandleReceivedData(result);
                }
            }
            catch (SocketException e)
            {
                await this.logger.Log($"[{nameof(SocketException)}] {e.Message}.\n{e.StackTrace}", this);
            }
            catch (Exception e)
            {
                await this.logger.Log($"[{nameof(Exception)}] {e.Message}.\n{e.StackTrace}", this);
            }
            finally
            {
                Running = false;
            }
        }

        public void Stop() => this.udpClient.Client.Disconnect(true);

        private async Task HandleReceivedData(UdpReceiveResult result)
        {
            string data = Encoding.UTF8.GetString(result.Buffer);
            await this.logger.Log($"Server visible: {data}", this);
            Debug.WriteLine($"IP Endpoint = {result.RemoteEndPoint.Address}");
            ServerData serverData = new(data, result.RemoteEndPoint.Address.ToString());
            this.onServerDataReceived(serverData);
        }
    }
}