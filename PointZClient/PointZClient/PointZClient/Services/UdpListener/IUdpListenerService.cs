using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PointZClient.Models.Server;

namespace PointZClient.Services.UdpListener
{
    public interface IUdpListenerService
    {
        Task StartAsync();
        void Stop();
        public bool Running { get; }
        public ObservableCollection<ServerData> Servers { get; }
    }
}