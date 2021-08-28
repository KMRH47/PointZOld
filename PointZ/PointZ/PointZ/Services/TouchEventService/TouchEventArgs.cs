namespace PointZ.Services.TouchEventService
{
    public class TouchEventArgs
    {
        public TouchEventArgs(float x, float y, TouchEventAction touchEventAction)
        {
            X = x;
            Y = y;
            TouchEventAction = touchEventAction;
        }

        public float X { get; }
        public float Y { get; }
        public TouchEventAction TouchEventAction { get; }
    }
}