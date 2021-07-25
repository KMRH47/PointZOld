using System;
using PointZClient.Services.TouchEventService;
using Xamarin.Forms;

[assembly: Dependency(typeof(PointZClient.Android.Services.TouchEventService))]

namespace PointZClient.Android.Services
{
    public class TouchEventService : ITouchEventService
    {
        public event EventHandler<TouchEventArgs> ScreenTouched;

        public void OnScreenTouched(float x, float y)
        {
            TouchEventArgs args = new TouchEventArgs {X = x, Y = y};
            RaiseScreenTouchedEvent(args);
        }

        private void RaiseScreenTouchedEvent(TouchEventArgs e) => this.ScreenTouched?.Invoke(this, e);
    }
}