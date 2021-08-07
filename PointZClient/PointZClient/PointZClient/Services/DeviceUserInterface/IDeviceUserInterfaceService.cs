using PointZClient.Models.DisplaySettings;
using PointZClient.Models.NavigationBar;

namespace PointZClient.Services.DeviceUserInterface
{
    public interface IDeviceUserInterfaceService
    {
        public NavigationBarData NavigationBar { get; set; }
        public DisplaySettingsData DisplaySettings { get; set; }
    }
}