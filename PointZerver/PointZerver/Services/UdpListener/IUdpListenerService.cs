using System.Threading;
using System.Threading.Tasks;

namespace PointZerver.Services.UdpListener
{
    public interface IUdpListenerService
    {
        Task StartAsync(CancellationToken token);
    }
}