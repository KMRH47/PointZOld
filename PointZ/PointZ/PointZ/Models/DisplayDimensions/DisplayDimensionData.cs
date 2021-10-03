namespace PointZ.Models.DisplayDimensions
{
    public readonly struct DisplayDimensionData
    {
        public DisplayDimensionData(double width, double height)
        {
            Width = width;
            Height = height;
        }
        
        public double Height { get; }
        public double Width { get; }
    }
}