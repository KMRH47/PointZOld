﻿using System;
using System.Threading.Tasks;
using PointZ.Models.Server;

namespace PointZ.Services.UdpListener
{
    public interface IUdpListenerService
    {
        Task StartAsync(Action<ServerData> onServerDataReceived);
        void Stop();
        public bool Running { get; }
    }
}