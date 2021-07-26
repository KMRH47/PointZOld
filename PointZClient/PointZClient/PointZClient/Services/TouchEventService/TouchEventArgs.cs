using PointZClient.Models.ScreenTouch;

namespace PointZClient.Services.TouchEventService
{
    public class TouchEventArgs
    {
        public TouchEventArgs(ScreenTouchData screenTouchData) => ScreenTouchData = screenTouchData;

        public ScreenTouchData ScreenTouchData { get; }
    }
}