using System.Threading;
using System.Threading.Tasks;

namespace PointZ.Services.UdpBroadcast
{
    public interface IUdpBroadcastService
    {
        Task StartAsync(CancellationToken cancellationToken);
    }
}