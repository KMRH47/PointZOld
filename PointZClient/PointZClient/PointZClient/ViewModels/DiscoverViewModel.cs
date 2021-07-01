using System.Collections.ObjectModel;
using PointZClient.Models.Server;
using PointZClient.Services.UdpListener;
using PointZClient.ViewModels.Base;

namespace PointZClient.ViewModels
{
    public class DiscoverViewModel : ViewModelBase
    {
        private readonly IUdpListenerService udpListenerService;

        public DiscoverViewModel(IUdpListenerService udpListenerService)
        {
            this.udpListenerService = udpListenerService;
            Start();
        }

        public void Start()
        {
            if (!this.udpListenerService.Running)
                _ = this.udpListenerService.StartAsync();
        }

        public void Stop()
        {
            if (this.udpListenerService.Running)
                this.udpListenerService.Stop();
        }

        private ObservableCollection<ServerData> Servers => this.udpListenerService.Servers;
        public bool IsSearching { get; set; } = true;
    }
}