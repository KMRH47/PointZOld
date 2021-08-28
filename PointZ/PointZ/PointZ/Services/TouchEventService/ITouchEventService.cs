using System;

namespace PointZ.Services.TouchEventService
{
    public interface ITouchEventService
    {
        event EventHandler<TouchEventArgs> OnScreenTouched;
        public void NotifyOnScreenTouched(float x, float y, TouchEventAction touchEventAction);
    }
}