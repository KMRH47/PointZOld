namespace PointZClient.Models.DeviceUserInterface
{
    public struct NavigationBarData
    {
        public NavigationBarData(double widthPixels, double heightPixels)
        {
            WidthPixels = widthPixels;
            HeightPixels = heightPixels;
        }

        public double WidthPixels { get; }
        public double HeightPixels { get; }
    }
}