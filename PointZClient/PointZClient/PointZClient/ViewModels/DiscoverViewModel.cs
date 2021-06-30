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
        }

        public bool IsSearching { get; set;  } = true;
        
    }
}