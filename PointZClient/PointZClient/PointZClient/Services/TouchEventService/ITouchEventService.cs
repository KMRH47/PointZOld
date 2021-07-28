using System;

namespace PointZClient.Services.TouchEventService
{
    public interface ITouchEventService
    {
        event EventHandler<TouchEventArgs> ScreenTouched;
        public void OnScreenTouched(float x, float y, TouchEventActions touchEventAction);
    }
}