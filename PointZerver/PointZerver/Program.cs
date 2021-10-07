using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using InputSimulatorStandard;
using PointZerver.Services.CommandConverter;
using PointZerver.Services.DataInterpreter;
using PointZerver.Services.Logger;
using PointZerver.Services.Simulators;
using PointZerver.Services.UdpBroadcast;
using PointZerver.Services.UdpListener;
using PointZerver.Tools;
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
            IPAddress ipAddress = NetworkTools.GetPhysicalNetworkInterfaceIpv4Address();

            if (ipAddress == null)
            {
                Console.WriteLine($"No physical network interface active, aborting...");
                ListenEscape();
                return Task.CompletedTask;
            }
            
            IPEndPoint listenerEndPoint = new (ipAddress, 45454);
            IPEndPoint broadcastDestEndPoint = new (ipAddress, 0);

            // Services
            IInputSimulator mouseSimulatorService = new MouseSimulatorService(new MouseSimulator(), logger);
            IInputSimulator keyboardSimulatorService = new KeyboardSimulatorService(new KeyboardSimulator(),
                new VirtualKeyCodeConverterService(), logger);
            IUdpBroadcastService udpBroadcastService = new UdpBroadcastService(new UdpClient(broadcastDestEndPoint), logger);
            IDataInterpreterService dataInterpreterService =
                new DataInterpreterService(logger, keyboardSimulatorService, mouseSimulatorService);
            IUdpListenerService udpListenerService =
                new UdpListenerService(new UdpClient(listenerEndPoint), dataInterpreterService, logger);

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

        private static void Welcome() => Console.WriteLine("PointZerver running!\n");

        private static void ListenEscape()
        {
            (int left, int top) = Console.GetCursorPosition();
            Console.WriteLine("Press escape to quit.");            

            while (true)
            {
                ConsoleKeyInfo readKey = Console.ReadKey();
                if (readKey.Key == ConsoleKey.Escape) return;
                Console.SetCursorPosition(left, top);
            }
        }
    }
}