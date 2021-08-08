using System.Threading;
using System.Threading.Tasks;

namespace PointZerver.Services.UdpBroadcast
{
    public interface IUdpBroadcastService
    {
        Task StartAsync(CancellationToken token, ushort port = 45455, int delayMs = 1000);
    }
}