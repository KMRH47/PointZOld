using PointZ.Models.DisplaySettings;
using PointZ.Models.NavigationBar;
using PointZ.Services.DeviceUserInterface;
using Xamarin.Forms;

[assembly: Dependency(typeof(PointZ.Android.Services.DeviceUserInterfaceService))]

namespace PointZ.Android.Services
{
    public class DeviceUserInterfaceService : IDeviceUserInterfaceService
    {
        public NavigationBarData NavigationBar { get; set; }
        public DisplaySettingsData DisplaySettings { get; set; }
    }
}