using PointZClient.ViewModels.Base;

namespace PointZClient.ViewModels
{
    public class DiscoverViewModel : ViewModelBase
    {
        
        public DiscoverViewModel()
        {
           // ActivityIndicator.IsRunning = true;
        }

        public bool IsSearching { get; set;  } = true;
    }
}