using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
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

        private void OnConnect() => this.NavigationService.NavigateToAsync<SessionViewModel>(SelectedServer.IpEndPoint);

        private void OnServerDataReceived(ServerData server)
        {
            if (IsServerAlreadyAdded(server)) return;
            Debug.WriteLine($"Address: {server.IpEndPoint.Address}");
            Debug.WriteLine($"Port: {server.IpEndPoint.Port}");
            Servers.Add(server);
        }

        private bool IsServerAlreadyAdded(ServerData server) =>
            Servers.Any(s => Equals(s.IpEndPoint.Address, server.IpEndPoint.Address));
    }
}