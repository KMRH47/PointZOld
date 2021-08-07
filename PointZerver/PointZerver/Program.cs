using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using InputSimulatorStandard;
using PointZ.Services.DataInterpreter;
using PointZ.Services.Logger;
using PointZ.Services.Simulators;
using PointZ.Services.UdpBroadcast;
using PointZ.Services.UdpListener;
using IInputSimulator = PointZ.Services.Simulators.IInputSimulator;

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

            // Services
            IInputSimulator mouseSimulatorService = new MouseSimulatorService(new MouseSimulator(), logger);
            IInputSimulator keyboardSimulatorService = new KeyboardSimulatorService(new KeyboardSimulator(), logger);
            IUdpBroadcastService udpBroadcastService = new UdpBroadcastService(new UdpClient(), logger);
            IDataInterpreterService dataInterpreterService =
                new DataInterpreterService(logger, keyboardSimulatorService, mouseSimulatorService);
            IUdpListenerService udpListenerService =
                new UdpListenerService(new UdpClient(45454), dataInterpreterService, logger);

            // Cancellation Tokens
            CancellationTokenSource udpBroadcastTokenSource = new();
            CancellationTokenSource udpListenerTokenSource = new();

            // Run
            Task broadcastServiceTask = udpBroadcastService.StartAsync(udpBroadcastTokenSource.Token);
            Task listenerServiceTask = udpListenerService.StartAsync(udpListenerTokenSource.Token);

            // udpListenerTokenSource.Cancel();

            Console.ReadKey();
        }
    }
}