using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using PointZerver.Services.Logger;
using PointZerver.Services.SimulatorInterpreter;

namespace PointZerver.Services.UdpListener
{
    public class UdpListenerService : IUdpListenerService
    {
        private const string TaskCancelledMessage = "The UDP Listener service was forcefully stopped.";
        private readonly UdpClient udpClient;
        private readonly ISimulatorInterpreterService simulatorInterpreterService;
        private readonly ILogger logger;

        public UdpListenerService(UdpClient udpClient, ISimulatorInterpreterService simulatorInterpreterService, ILogger logger)
        {
            this.udpClient = udpClient;
            this.simulatorInterpreterService = simulatorInterpreterService;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken token)
        {
            try
            {
                EndPoint endPoint = this.udpClient.Client.LocalEndPoint;
                await this.logger.Log($"Listening on '{endPoint}'.", this);

                while (true)
                {
                    UdpReceiveResult udpReceiveResult = await this.udpClient.ReceiveAsync();
                   _ = this.simulatorInterpreterService.InterpretAsync(udpReceiveResult.Buffer);
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
                await this.logger.Log($"[{nameof(Exception)}] {e.Message}", this);
            }
        }
    }
}