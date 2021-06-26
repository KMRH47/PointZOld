using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using PointZ.Services.DataInterpreter;
using PointZ.Services.Logger;
using PointZ.Services.NetTools;

namespace PointZ.Services.UdpListener
{
    public class UdpListenerService : IUdpListenerService
    {
        private const string TaskCancelledMessage = "The UDP Listener service was forcefully stopped.";
        private readonly UdpClient udpClient;
        private readonly INetToolsService netToolsService;
        private readonly IDataInterpreterService dataInterpreterService;
        private readonly ILogger logger;

        public UdpListenerService(UdpClient udpClient, INetToolsService netToolsService,
            IDataInterpreterService dataInterpreterService, ILogger logger)
        {
            this.udpClient = udpClient;
            this.netToolsService = netToolsService;
            this.dataInterpreterService = dataInterpreterService;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                string localIpv4Address = await this.netToolsService.GetLocalIpv4Address(cancellationToken);
                string hostNameAndIpAddress = $"{localIpv4Address}:45454";
                await this.logger.Log($"Listening on '{hostNameAndIpAddress}'", this);

                while (!cancellationToken.IsCancellationRequested)
                {
                    UdpReceiveResult result = await this.udpClient.ReceiveAsync();
                    _ = this.dataInterpreterService.InterpretAsync(result.Buffer);
                }
            }
            catch (TaskCanceledException)
            {
                await this.logger.Log(TaskCancelledMessage, this);
            }
            catch (SocketException e)
            {
                await this.logger.Log($"{e.Message}", this);
            }
            catch (Exception e)
            {
                await this.logger.Log($"Stopped {e.Message}", this);
            }
        }
    }
}