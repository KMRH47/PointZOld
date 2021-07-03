using System;
using System.Collections.ObjectModel;
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

        public UdpListenerService(UdpClient udpClient, ILogger logger)
        {
            this.udpClient = udpClient;
            this.logger = logger;
        }

        public void Stop() => this.udpClient.Client.Disconnect(true);
        public bool Running { get; private set; }
        private ObservableCollection<ServerData> Servers { get; set; }

        public async Task StartAsync(ObservableCollection<ServerData> servers)
        {
            try
            {
                Servers = servers;
                Running = true;
                IPEndPoint endPoint = new(IPAddress.Any, 45455);
                this.udpClient.Client.Bind(endPoint);

                while (true)
                {
                    UdpReceiveResult result = await this.udpClient.ReceiveAsync();
                    _ = HandleResult(result);
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

        private async Task HandleResult(UdpReceiveResult result)
        {
            string data = Encoding.UTF8.GetString(result.Buffer);
            await this.logger.Log($"Handling result: {data}", this);
            string[] dataSplit = data.Split('|');
            ServerData serverData = new(dataSplit[0], dataSplit[1]);
            await UpdateServer(serverData);
        }

        private Task UpdateServer(ServerData serverData)
        {
            if (Servers.Count > 0)
            {
                for (int i = 0; i < Servers.Count; i++)
                {
                    if (serverData.Name != Servers[i].Name) continue;

                    this.logger.Log(
                        $"Updated {Servers[i].Name}|{Servers[i].Address} to {serverData.Name}|{serverData.Address}.",
                        this);
                    Servers[i] = serverData;
                    return Task.CompletedTask;
                }
            }

            Servers.Add(serverData);
            this.logger.Log($"Server {serverData.Name}|{serverData.Address} added.", this);
            return Task.CompletedTask;
        }
    }
}