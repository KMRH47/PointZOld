namespace PointZClient.Services.TouchEventService
{
    public class TouchEventArgs
    {
        public TouchEventArgs(float x, float y, TouchEventActions touchEventAction)
        {
            X = x;
            Y = y;
            TouchEventAction = touchEventAction;
        }

        public float X { get; }
        public float Y { get; }
        public TouchEventActions TouchEventAction { get; }
    }
}