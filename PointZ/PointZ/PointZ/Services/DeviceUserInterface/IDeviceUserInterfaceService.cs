using PointZ.Models.DisplaySettings;
using PointZ.Models.NavigationBar;

namespace PointZ.Services.DeviceUserInterface
{
    public interface IDeviceUserInterfaceService
    {
        public NavigationBarData NavigationBar { get; set; }
        public DisplaySettingsData DisplaySettings { get; set; }
    }
}