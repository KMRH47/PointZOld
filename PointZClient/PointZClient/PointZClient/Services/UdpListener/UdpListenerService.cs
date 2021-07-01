using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PointZClient.Extensions;
using PointZClient.Models.Server;
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

        public void Stop() => this.udpClient.Client.Disconnect(true);
        public bool Running { get; private set; }
        public ObservableCollection<ServerData> Servers { get; } = new();

        public async Task StartAsync()
        {
            try
            {
                Running = true;
                string localIpv4Address = await NetworkTools.GetLocalIpv4Address();
                int port = this.udpClient.GetBoundPort();
                string hostNameAndIpAddress = $"{localIpv4Address}:{port}";
                await this.logger.Log($"Listening on '{hostNameAndIpAddress}'.", this);

                while (true)
                {
                    UdpReceiveResult result = await this.udpClient.ReceiveAsync();
                    _ = HandleResult(result);
            
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
            finally
            {
                Running = false;
            }
        }

        private async Task HandleResult(UdpReceiveResult result)
        {
            string data = Encoding.UTF8.GetString(result.Buffer);
            string[] dataSplit = data.Split('|');
            ServerData serverData = new(dataSplit[0], dataSplit[1]);
            await UpdateServer(serverData);
        }

        private Task UpdateServer(ServerData serverData)
        {
            for (int i = 0; i < Servers.Count; i++)
            {
                if (serverData.Name == Servers[i].Name)
                    Servers[i] = serverData;

                Servers.Add(serverData);
            }

            return Task.CompletedTask;
        }
        
        /*
         * 
                if (DateTime.Now.Second - serverData.LastUpdated.Second > 5 )
                {
                    
                }
                
         */
    }
}