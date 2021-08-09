using System;
using System.Threading.Tasks;
using PointZClient.Models.Server;

namespace PointZClient.Services.UdpListener
{
    public interface IUdpListenerService
    {
        Task StartAsync(Action<ServerData> onServerDataReceived);
        void Stop();
        public bool Running { get; }
    }
}