using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using PointZClient.Models.Server;
using PointZClient.Services.UdpListener;
using PointZClient.ViewModels.Base;
using Xamarin.Forms;

namespace PointZClient.ViewModels
{
    public class DiscoverViewModel : ViewModelBase
    {
        private bool anyServerFound = true;
        private bool isSearching = true;
        private bool isServerSelected;
        private ServerData selectedServer;

        private readonly IUdpListenerService udpListenerService;

        public DiscoverViewModel(IUdpListenerService udpListenerService)
        {
            this.udpListenerService = udpListenerService;
            ConnectCommand = new Command(OnConnect);
            udpListenerService.StartAsync(OnServerDataReceived);
        }

        public ObservableCollection<ServerData> Servers { get; } = new();
        public ServerData SelectedServer
        {
            get => this.selectedServer;
            set
            {
                this.selectedServer = value;
                IsServerSelected = true;
            }
        }

        public ICommand ConnectCommand { get; private set; }

        public bool IsServerSelected
        {
            get => this.isServerSelected;
            private set
            {
                this.isServerSelected = value;
                RaisePropertyChanged(() => IsServerSelected);
            }
        }
        

        public bool AnyServerFound
        {
            get => this.anyServerFound;
            private set
            {
                this.anyServerFound = value;
                RaisePropertyChanged(() => AnyServerFound);
            }
        }

        public bool IsSearching
        {
            get => this.isSearching;
            set
            {
                this.isSearching = value;
                RaisePropertyChanged(() => IsSearching);
            }
        }

        public void Start()
        {
            if (!this.udpListenerService.Running)
                _ = this.udpListenerService.StartAsync(OnServerDataReceived);
        }

        public void Stop()
        {
            if (this.udpListenerService.Running)
                this.udpListenerService.Stop();
        }

        private void OnConnect()
        {
            this.NavigationService.NavigateToAsync<SessionViewModel>();
        }

        private void OnServerDataReceived(ServerData server)
        {
            if (IsServerAlreadyAdded(server)) return;
            Servers.Add(server);
            AnyServerFound = Servers.Count <= 0;
        }

        private bool IsServerAlreadyAdded(ServerData server) => Servers.Any(s => s.Address == server.Address);
    }
}