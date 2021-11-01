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
            ServerData serverData = new ("T", server.IpEndPoint);
            ServerData serverData2 = new ("Test", server.IpEndPoint);
            ServerData serverData3 = new ("Testing This", server.IpEndPoint);
            ServerData serverData4 = new ("1A2B3C4D5E6F ZZXXYYÆØÅ", server.IpEndPoint);
            ServerData serverData5 = new ("This Is Very Weird... Why Write like this....", server.IpEndPoint);
            ServerData serverData6 = new ("Filling", server.IpEndPoint);
            ServerData serverData7 = new ("Filling 2", server.IpEndPoint);
            ServerData serverData8 = new ("Filling 3", server.IpEndPoint);
            ServerData serverData9 = new ("Filling 4", server.IpEndPoint);
            ServerData serverData0 = new ("Filling 5", server.IpEndPoint);
            Servers.Add(server);
            Servers.Add(serverData );
            Servers.Add(serverData2);
            Servers.Add(serverData3);
            Servers.Add(serverData4);
            Servers.Add(serverData5);
            Servers.Add(serverData6);
            Servers.Add(serverData7);
            Servers.Add(serverData8);
            Servers.Add(serverData9);
            Servers.Add(serverData0);
        }

        private bool IsServerAlreadyAdded(ServerData server) =>
            Servers.Any(s => Equals(s.IpEndPoint.Address, server.IpEndPoint.Address));
    }
}