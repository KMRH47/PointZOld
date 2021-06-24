using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using PointZ.Services.Logger;
using PointZ.Services.UdpBroadcast;

namespace PointZ
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            // Cancellation Token
            CancellationTokenSource broadcastServiceTokenSource = new();

            // Services
            ILogger logger = new FileLogger("Main");
#if RELEASE
            IUdpBroadcastService udpBroadcastService = new UdpBroadcastService(new UdpClient(), new ConsoleLogger());
#else
            IUdpBroadcastService udpBroadcastService = new UdpBroadcastService(new UdpClient(), new FileLogger("UDPBroadcastService"));
#endif

            // Run
            Task broadcastServiceTask = udpBroadcastService.StartAsync(broadcastServiceTokenSource.Token);

            while (true)
            {
                string input = Console.ReadLine();

                if (input == "kill") break;
                await logger.Log($"{input}");
            }
        }
    }
}