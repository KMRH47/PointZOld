using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using PointZ.Services.LoggerService;
using PointZ.Services.UdpBroadcastService;

namespace PointZ
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            // Logger
            ILogger consoleLogger = new ConsoleLogger();

            // UDP Broadcaster
            IPEndPoint ipEndPoint = new(IPAddress.Any, 45454);
            UdpClient udpClient = new(ipEndPoint);
            IUdpBroadcastService udpBroadcastService = new UdpBroadcastService(udpClient, consoleLogger);

            IPEndPoint sender = new(0, 0);
            UdpClient udpClient2 = new(sender);
            byte[] message = Encoding.UTF8.GetBytes("Hello there...!");

            // Begin
            udpBroadcastService.StartAsync();
            Thread.Sleep(2000);
            consoleLogger.Log("Sending message...!");
            udpClient2.Connect(new IPEndPoint(IPAddress.Loopback, 45454));
            udpClient2.Send(message, message.Length);

            Console.ReadKey();
        }
    }
}