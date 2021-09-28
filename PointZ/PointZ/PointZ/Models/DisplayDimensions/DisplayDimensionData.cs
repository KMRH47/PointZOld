namespace PointZ.Models.DisplayDimensions
{
    public readonly struct DisplayDimensionData
    {
        public DisplayDimensionData(int width, int height)
        {
            Width = width;
            Height = height;
        }
        
        public int Height { get; }
        public int Width { get; }
    }
}