namespace PointZ.Models.NavigationBarDimension
{
    public struct NavigationBarDimensionData
    {
        public NavigationBarDimensionData(double widthPixels, double heightPixels)
        {
            WidthPixels = widthPixels;
            HeightPixels = heightPixels;
        }

        public double WidthPixels { get; }
        public double HeightPixels { get; }
    }
}