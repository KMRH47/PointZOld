using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using InputSimulatorStandard;
using PointZerver.Services.DataInterpreter;
using PointZerver.Services.Logger;
using PointZerver.Services.Simulators;
using PointZerver.Services.UdpBroadcast;
using PointZerver.Services.UdpListener;
using IInputSimulator = PointZerver.Services.Simulators.IInputSimulator;

namespace PointZerver
{
    internal static class Program
    {
        private static readonly ConsoleLogger ConsoleLogger = new();

        private static Task Main(string[] args)
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
            
            Welcome();
            ListenEscape();
            return Task.CompletedTask;
        }

        private static void Welcome() => Console.WriteLine("PointZerver running!\n" + "Press escape to quit.");

        private static void ListenEscape()
        {
            (int left, int top) = Console.GetCursorPosition();

            while (true)
            {
                ConsoleKeyInfo readKey = Console.ReadKey();
                if (readKey.Key == ConsoleKey.Escape) return;
                Console.SetCursorPosition(left, top);
            }
        }
    }
}