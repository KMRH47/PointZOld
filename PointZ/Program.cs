﻿using System;
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
            CancellationTokenSource cancellationTokenSource = new();

            // Logger
            ILogger logger = new ConsoleLogger();

            // UDP Broadcast Service
            UdpClient udpClient = new();
            IUdpBroadcastService udpBroadcastService = new UdpBroadcastService(udpClient, logger);

            // Run tasks
            Task broadcastServiceTask = udpBroadcastService.StartAsync(cancellationTokenSource.Token);

            Console.ReadKey();
        }
    }
}