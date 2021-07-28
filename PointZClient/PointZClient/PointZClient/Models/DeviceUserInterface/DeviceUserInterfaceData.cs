namespace PointZClient.Models.DeviceUserInterface
{
    public struct DeviceUserInterfaceData
    {
        public DeviceUserInterfaceData(NavigationBarData navigationBarData)
        {
            NavigationBarData = navigationBarData;
        }
        
        public NavigationBarData NavigationBarData { get; set; }
    }
}