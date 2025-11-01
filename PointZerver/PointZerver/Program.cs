using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using InputSimulatorStandard;
using Microsoft.Extensions.DependencyInjection;
using PointZerver.Services.Logger;
using PointZerver.Services.SimulatorInterpreter;
using PointZerver.Services.Simulators;
using PointZerver.Services.Simulators.Controllers;
using PointZerver.Services.UdpBroadcast;
using PointZerver.Services.UdpListener;
using PointZerver.Services.VirtualKeyCodeMapper;
using PointZerver.Tools;

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

            // Services
            // -Pre-registration
            IPEndPoint serverIpEndPoint = new(ipAddress, 45454);
            UdpClient udpClient = new();
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpClient.Client.Bind(serverIpEndPoint);
            IVirtualKeyCodeMapperService virtualKeyCodeMapperService = new VirtualKeyCodeMapperService();
            bool isMac = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

            // -Registration
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(udpClient);
            services.AddSingleton(logger);
            services.AddSingleton<IVirtualKeyCodeMapperService>(virtualKeyCodeMapperService);
            if (isMac)
            {
                services.AddSingleton<IMouseController, MacMouseController>();
                services.AddSingleton<IKeyboardController, MacKeyboardController>();
            }
            else
            {
                IMouseSimulator mouseSimulator = new MouseSimulator();
                IKeyboardSimulator keyboardSimulator = new KeyboardSimulator();
                services.AddSingleton(mouseSimulator);
                services.AddSingleton(keyboardSimulator);
                services.AddSingleton<IMouseController>(
                    _ => new WindowsMouseController(mouseSimulator));
                services.AddSingleton<IKeyboardController>(
                    provider => new WindowsKeyboardController(keyboardSimulator,
                        provider.GetRequiredService<IVirtualKeyCodeMapperService>()));
            }
            services.AddScoped<MouseSimulatorService>();
            services.AddScoped<KeyboardSimulatorService>();
            services.AddScoped<IUdpBroadcastService, UdpBroadcastService>();
            services.AddScoped<ISimulatorInterpreterService>(provider =>
            {
                ILogger logger = provider.GetRequiredService<ILogger>();
                MouseSimulatorService mouseService = provider.GetRequiredService<MouseSimulatorService>();
                KeyboardSimulatorService keyboardService = provider.GetRequiredService<KeyboardSimulatorService>();
                return new SimulatorInterpreterService(logger, mouseService, keyboardService);
            });
            services.AddScoped<IUdpListenerService, UdpListenerService>();
            services.AddScoped<IInputSimulatorService, MouseSimulatorService>();
            services.AddScoped<IInputSimulatorService, KeyboardSimulatorService>();

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            IUdpBroadcastService udpBroadcastService = serviceProvider.GetService<IUdpBroadcastService>();
            IUdpListenerService udpListenerService = serviceProvider.GetService<IUdpListenerService>();

            // Cancellation Tokens
            CancellationTokenSource udpBroadcastTokenSource = new();
            CancellationTokenSource udpListenerTokenSource = new();

            // Run
            Task broadcastServiceTask = udpBroadcastService.StartAsync(udpBroadcastTokenSource.Token);
            Task listenerServiceTask = udpListenerService.StartAsync(udpListenerTokenSource.Token);

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
