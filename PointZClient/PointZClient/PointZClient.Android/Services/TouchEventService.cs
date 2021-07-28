using System;
using PointZClient.Services.TouchEventService;
using Xamarin.Forms;

[assembly: Dependency(typeof(PointZClient.Android.Services.TouchEventService))]

namespace PointZClient.Android.Services
{
    public class TouchEventService : ITouchEventService
    {
        public event EventHandler<TouchEventArgs> ScreenTouched;

        public void OnScreenTouched(float x, float y, TouchEventActions touchEventAction)
        {
            TouchEventArgs args = new TouchEventArgs(x, y, touchEventAction);
            RaiseScreenTouchedEvent(args);
        }

        private void RaiseScreenTouchedEvent(TouchEventArgs e) => this.ScreenTouched?.Invoke(this, e);
    }
}