using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PointZClient.Models.Server;

namespace PointZClient.Services.UdpListener
{
    public interface IUdpListenerService
    {
        Task StartAsync(ObservableCollection<ServerData> servers);
        void Stop();
        public bool Running { get; }
    }
}