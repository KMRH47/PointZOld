using PointZClient.Models.DisplaySettings;
using PointZClient.Models.NavigationBar;
using PointZClient.Services.DeviceUserInterface;
using Xamarin.Forms;

[assembly: Dependency(typeof(PointZClient.Android.Services.DeviceUserInterfaceService))]

namespace PointZClient.Android.Services
{
    public class DeviceUserInterfaceService : IDeviceUserInterfaceService
    {
        public NavigationBarData NavigationBar { get; set; }
        public DisplaySettingsData DisplaySettings { get; set; }
    }
}