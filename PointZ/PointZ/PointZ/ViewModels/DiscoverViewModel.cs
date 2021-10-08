using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using PointZ.Models.Server;
using PointZ.Services.UdpListener;
using PointZ.ViewModels.Base;
using Xamarin.Forms;

namespace PointZ.ViewModels
{
    public class DiscoverViewModel : ViewModelBase
    {
        private readonly IUdpListenerService udpListenerService;

        private bool isSearching = true;
        private bool isServerSelected;
        
        private ServerData selectedServer;

        public DiscoverViewModel(IUdpListenerService udpListenerService)
        {
            this.udpListenerService = udpListenerService;
            ConnectCommand = new Command(OnConnect);
            udpListenerService.StartAsync(OnServerDataReceived);
        }
        
        public ICommand ConnectCommand { get; }

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

        public bool IsServerSelected
        {
            get => this.isServerSelected;
            private set
            {
                this.isServerSelected = value;
                OnPropertyChanged();
            }
        }

        public bool IsSearching
        {
            get => this.isSearching;
            set
            {
                this.isSearching = value;
                OnPropertyChanged();
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
            this.NavigationService.NavigateToAsync<SessionViewModel>(SelectedServer);
        }

        private void OnServerDataReceived(ServerData server)
        {
            if (IsServerAlreadyAdded(server)) return;
            Servers.Add(server);
        }

        private bool IsServerAlreadyAdded(ServerData server) => Servers.Any(s => s.IpEndPoint == server.IpEndPoint);
    }
}