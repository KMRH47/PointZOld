﻿using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using PointZerver.Extensions;
using PointZerver.Services.DataInterpreter;
using PointZerver.Services.Logger;
using PointZerver.Tools;

namespace PointZerver.Services.UdpListener
{
    public class UdpListenerService : IUdpListenerService
    {
        private const string TaskCancelledMessage = "The UDP Listener service was forcefully stopped.";
        private readonly UdpClient udpClient;
        private readonly IDataInterpreterService dataInterpreterService;
        private readonly ILogger logger;

        public UdpListenerService(UdpClient udpClient, IDataInterpreterService dataInterpreterService, ILogger logger)
        {
            this.udpClient = udpClient;
            this.dataInterpreterService = dataInterpreterService;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken token)
        {
            try
            {
                string localIpv4Address = await NetworkTools.GetLocalIpv4Address(token);
                int port = this.udpClient.GetBoundPort();
                string hostNameAndIpAddress = $"{localIpv4Address}:{port}";
                await this.logger.Log($"Listening on '{hostNameAndIpAddress}'.", this);

                while (true)
                {
                    byte[] bytes = new byte[200];
                    await this.udpClient.Client.ReceiveAsync(bytes, SocketFlags.None, token);
                    _ = this.dataInterpreterService.InterpretAsync(bytes);
                }
            }
            catch (TaskCanceledException)
            {
                await this.logger.Log(TaskCancelledMessage, this);
            }
            catch (OperationCanceledException)
            {
                await this.logger.Log(TaskCancelledMessage, this);
            }
            catch (SocketException e)
            {
                await this.logger.Log($"[{nameof(SocketException)}] {e.Message}", this);
            }
            catch (Exception e)
            {
                await this.logger.Log($"[{nameof(Exception)}] {e.Message}", this);
            }
        }
    }
}