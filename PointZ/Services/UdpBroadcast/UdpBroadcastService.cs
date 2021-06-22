using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using PointZ.Services.DataInterpreter;
using PointZ.Services.Logger;

namespace PointZ.Services.UdpBroadcast
{
    public class UdpBroadcastService : IUdpBroadcastService
    {
        private readonly UdpClient udpClient;
        private readonly IDataInterpreter dataInterpreter;
        private readonly ILogger logger;

        public UdpBroadcastService(UdpClient udpClient, IDataInterpreter dataInterpreter, ILogger logger)
        {
            this.udpClient = udpClient;
            this.dataInterpreter = dataInterpreter;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                this.logger.Log("[UDP Broadcaster] Listening...");
                
              //  cancellationToken.re
                while (!cancellationToken.IsCancellationRequested)
                {
                    UdpReceiveResult result = await this.udpClient.ReceiveAsync();
                    _ = this.dataInterpreter.InterpretAsync(result.Buffer);
                }
                
                this.logger.Log("DEAD :(");
            }
            catch (Exception e)
            {
                this.logger.Log(e.Message);
            }
        }
    }
}