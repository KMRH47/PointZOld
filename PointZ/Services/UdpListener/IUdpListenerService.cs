using System.Threading;
using System.Threading.Tasks;

namespace PointZ.Services.UdpListener
{
    public interface IUdpListenerService
    {
        Task StartAsync(CancellationToken cancellationToken);
    }
}