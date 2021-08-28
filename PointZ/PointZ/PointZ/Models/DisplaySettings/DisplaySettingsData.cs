namespace PointZ.Models.DisplaySettings
{
    public readonly struct DisplaySettingsData
    {
        public DisplaySettingsData(int width, int height)
        {
            Width = width;
            Height = height;
        }
        
        public int Height { get; }
        public int Width { get; }
    }
}