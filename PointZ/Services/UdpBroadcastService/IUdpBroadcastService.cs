using System.Threading.Tasks;

namespace PointZ.Services.UdpBroadcastService
{
    public interface IUdpBroadcastService
    {
        Task StartAsync();
    }
}