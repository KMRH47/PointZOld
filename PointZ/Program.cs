using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using PointZ.Services.DataInterpreter;
using PointZ.Services.Logger;
using PointZ.Services.UdpBroadcast;
using PointZ.Services.UdpListener;

namespace PointZ
{
    internal static class Program
    {
        private static readonly ConsoleLogger ConsoleLogger = new();

        private static async Task Main(string[] args)
        {
            // ReSharper disable once JoinDeclarationAndInitializer
            ILogger logger;
#if RELEASE
            logger = new FileLogger(ConsoleLogger);
#else
            logger = ConsoleLogger;
#endif

            // Cancellation Tokens
            CancellationTokenSource udpBroadcastTokenSource = new();
            CancellationTokenSource udpListenerTokenSource = new();

            // Services
            IUdpBroadcastService udpBroadcastService = new UdpBroadcastService(new UdpClient(), logger);
            IDataInterpreterService dataInterpreterService = new DataInterpreterService(logger);
            IUdpListenerService udpListenerService =
                new UdpListenerService(new UdpClient(45454), dataInterpreterService, logger);

            // Run
            Task broadcastServiceTask = udpBroadcastService.StartAsync(udpBroadcastTokenSource.Token);
            Task listenerServiceTask = udpListenerService.StartAsync(udpListenerTokenSource.Token);
            
            // udpListenerTokenSource.Cancel();

            Console.ReadKey();
        }
    }
}