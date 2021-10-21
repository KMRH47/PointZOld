using System;
using PointZ.Models.PlatformEvent;

namespace PointZ.Services.PlatformEventService
{
    public interface IPlatformEventService
    {
        event EventHandler OnViewAppearing;
        event EventHandler OnViewDisappearing;
        event EventHandler OnBackButtonPressed;
        event EventHandler<KeyEventArgs> OnCustomEntryKeyPress;
        public event EventHandler<TouchEventArgs> OnScreenTouched;
        void NotifyOnBackButtonPressed();
        void NotifyOnViewDisappearing();
        void NotifyOnViewAppearing();
        void NotifyOnCustomEntryKeyPress(KeyEventArgs e);
        void NotifyOnScreenTouched(float x, float y, TouchAction touchAction);
    }
}