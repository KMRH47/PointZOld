using System.Threading.Tasks;

namespace PointZClient.Services.UdpListener
{
    public interface IUdpListenerService
    {
        Task StartAsync();
    }
}