using PointZClient.Models.DeviceUserInterface;

namespace PointZClient.Services.DeviceUserInterface
{
    public interface IDeviceUserInterfaceService
    {
        public NavigationBarData NavigationBar { get; set; }
    }
}