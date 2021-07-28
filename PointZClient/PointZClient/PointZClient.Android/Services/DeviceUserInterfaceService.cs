using PointZClient.Models.DeviceUserInterface;
using PointZClient.Services.DeviceUserInterface;
using Xamarin.Forms;

[assembly: Dependency(typeof(PointZClient.Android.Services.DeviceUserInterfaceService))]

namespace PointZClient.Android.Services
{
    public class DeviceUserInterfaceService : IDeviceUserInterfaceService
    {
        public NavigationBarData NavigationBar { get; set; }
    }
}