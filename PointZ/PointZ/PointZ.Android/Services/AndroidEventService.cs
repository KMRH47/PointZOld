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
        public event EventHandler<KeyEventArgs> OnCustomEditorBackPressed;
        public event EventHandler OnCustomEditorFocusRequested;

        public void NotifyOnBackButtonPressed() => this.OnBackButtonPressed?.Invoke(this, EventArgs.Empty);
        public void NotifyOnViewDisappearing() => this.OnViewDisappearing?.Invoke(this, EventArgs.Empty);
        public void NotifyOnViewAppearing() => this.OnViewAppearing?.Invoke(this, EventArgs.Empty);
        public void NotifyOnCustomEditorBackPressed(KeyEventArgs e) => this.OnCustomEditorBackPressed?.Invoke(this, e);

        public void NotifyOnScreenTouched(float x, float y, TouchAction touchAction)
        {
            TouchEventArgs args = new(x, y, touchAction);
            this.OnScreenTouched?.Invoke(this, args);
        }

        public void NotifyOnCustomEditorFocusRequested() =>
            this.OnCustomEditorFocusRequested?.Invoke(this, EventArgs.Empty);
    }
}