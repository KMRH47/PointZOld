using System;
using PointZ.Models.PlatformEvent;
using PointZ.Services.PlatformEventService;

namespace PointZ.Android.Services
{
    public class AndroidEventService : IPlatformEventService
    {
        public event EventHandler OnViewAppearing;
        public event EventHandler OnViewDisappearing;
        public event EventHandler OnBackButtonPressed;
        public event EventHandler<TouchEventArgs> OnScreenTouched;
        public event EventHandler<KeyEventArgs> OnCustomEntryKeyPress;

        public void NotifyOnBackButtonPressed() => this.OnBackButtonPressed?.Invoke(this, EventArgs.Empty);
        public void NotifyOnViewDisappearing() => this.OnViewDisappearing?.Invoke(this, EventArgs.Empty);
        public void NotifyOnViewAppearing() => this.OnViewAppearing?.Invoke(this, EventArgs.Empty);
        public void NotifyOnCustomEntryKeyPress(KeyEventArgs e) => this.OnCustomEntryKeyPress?.Invoke(this, e);

        public void NotifyOnScreenTouched(float x, float y, TouchAction touchAction)
        {
            TouchEventArgs args = new(x, y, touchAction);
            this.OnScreenTouched?.Invoke(this, args);
        }
    }
}