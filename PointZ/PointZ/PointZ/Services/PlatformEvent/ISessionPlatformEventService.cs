using System;
using PointZ.Models.PlatformEvent;

namespace PointZ.Services.PlatformEvent
{
    public interface ISessionPlatformEventService
    {
        event EventHandler OnViewAppearing;
        event EventHandler OnViewDisappearing;
        event EventHandler OnBackButtonPressed;
        event EventHandler<TouchEventArgs> OnScreenTouched;
        event EventHandler<KeyEventArgs> OnKeyDown;
        void NotifyOnScreenTouched(float x, float y, TouchAction touchAction);
        void NotifyOnBackButtonPressed();
        void NotifyOnViewDisappearing();
        void NotifyOnViewAppearing();
        void NotifyOnKeyDown(KeyAction keyAction, string data);
    }
}