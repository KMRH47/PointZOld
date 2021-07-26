using System;
namespace PointZClient.Services.TouchEventService
{
    public interface ITouchEventService
    {
        event EventHandler<TouchEventArgs> ScreenTouched;
        void OnScreenTouched(float x, float y);
    }
}