using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using PointZClient.Models.Server;
using PointZClient.Services.UdpListener;
using PointZClient.ViewModels.Base;

namespace PointZClient.ViewModels
{
    public class DiscoverViewModel : ViewModelBase
    {
        private bool shouldSearch = true;
        
            
        private readonly IUdpListenerService udpListenerService;

        public DiscoverViewModel(IUdpListenerService udpListenerService)
        {
            this.udpListenerService = udpListenerService;
            udpListenerService.StartAsync(Servers);
            Servers.CollectionChanged += OnServersChanged;
        }

        public ObservableCollection<ServerData> Servers { get; } = new();
        public bool ShouldSearch
        {
            get => this.shouldSearch;
            private set
            {
                this.shouldSearch = value;
                RaisePropertyChanged(() => ShouldSearch);
            }
        }
        public bool IsSearching { get; set; } = true;

        public void Start()
        {
            if (!this.udpListenerService.Running)
                _ = this.udpListenerService.StartAsync(Servers);
        }

        public void Stop()
        {
            if (this.udpListenerService.Running)
                this.udpListenerService.Stop();
        }

        private void OnServersChanged(object o, NotifyCollectionChangedEventArgs eventArgs)
        {
            Debug.WriteLine($"Server count: {Servers.Count}\nShouldSearch = {ShouldSearch}");
            if (Servers.Count > 0)
                ShouldSearch = false;
            Debug.WriteLine($"ShouldSearch after  {ShouldSearch}");

        }
    }
}