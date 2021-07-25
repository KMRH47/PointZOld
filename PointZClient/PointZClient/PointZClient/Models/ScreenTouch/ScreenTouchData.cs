namespace PointZClient.Models.ScreenTouch
{
    public struct ScreenTouchData
    {
        public ScreenTouchData(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; }
        public float Y { get;  }
    }
}