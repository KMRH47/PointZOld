using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using PointZ.Services.DataInterpreter;
using PointZ.Services.Logger;

namespace PointZ.Services.UdpListener
{
    public class UdpListenerService : IUdpListenerService
    {
        private readonly UdpClient udpClient;
        private readonly IDataInterpreterService dataInterpreterService;
        private readonly ILogger logger;

        public UdpListenerService(UdpClient udpClient, IDataInterpreterService dataInterpreterService, ILogger logger)
        {
            this.udpClient = udpClient;
            this.dataInterpreterService = dataInterpreterService;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {

            while (!cancellationToken.IsCancellationRequested)
            {
                await this.udpClient.ReceiveAsync();
            }

        }
    }
}