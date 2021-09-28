using System;
using PointZ.Models.PlatformEvent;
using PointZ.Services.PlatformEvent;
using Xamarin.Forms;

[assembly: Dependency(typeof(PointZ.Android.Services.SessionAndroidEventService))]

namespace PointZ.Android.Services
{
    public class SessionAndroidEventService : ISessionPlatformEventService
    {
        public event EventHandler OnViewAppearing;
        public event EventHandler OnViewDisappearing;
        public event EventHandler OnBackButtonPressed;
        public event EventHandler<TouchEventArgs> OnScreenTouched;
        public event EventHandler<KeyEventArgs> OnKeyDown;

        public void NotifyOnBackButtonPressed() => this.OnBackButtonPressed?.Invoke(this, EventArgs.Empty);
        public void NotifyOnViewDisappearing() => this.OnViewDisappearing?.Invoke(this, EventArgs.Empty);
        public void NotifyOnViewAppearing() => this.OnViewAppearing?.Invoke(this, EventArgs.Empty);

        public void NotifyOnKeyDown(KeyAction keyAction, string data)
        {
            KeyEventArgs args = new(keyAction, data);
            this.OnKeyDown?.Invoke(this, args);
        }

        public void NotifyOnScreenTouched(float x, float y, TouchAction touchAction)
        {
            TouchEventArgs args = new(x, y, touchAction);
            this.OnScreenTouched?.Invoke(this, args);
        }
    }
}