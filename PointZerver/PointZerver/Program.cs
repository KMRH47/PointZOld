using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
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
        private static bool _windowVisible = true;
        private static readonly ConsoleLogger ConsoleLogger = new();

        private static async Task Main(string[] args)
        {
            IntPtr hWnd = FindWindow(null, Console.Title);

            if (hWnd != IntPtr.Zero) ShowWindow(hWnd, _windowVisible ? 1 : 0);

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

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }
}