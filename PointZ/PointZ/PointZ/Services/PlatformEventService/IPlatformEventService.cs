using System;
using PointZ.Models.PlatformEvent;

namespace PointZ.Services.PlatformEventService
{
    public interface IPlatformEventService
    {
        event EventHandler OnViewAppearing;
        event EventHandler OnViewDisappearing;
        event EventHandler OnBackButtonPressed;
        event EventHandler<TouchEventArgs> OnScreenTouched;
        event EventHandler OnCustomEditorFocusRequested;
        public event EventHandler<KeyEventArgs> OnCustomEditorBackPressed;
        void NotifyOnBackButtonPressed();
        void NotifyOnViewDisappearing();
        void NotifyOnViewAppearing();
        void NotifyOnScreenTouched(float x, float y, TouchAction touchAction);
        void NotifyOnCustomEditorBackPressed(KeyEventArgs e);
        void NotifyOnCustomEditorFocusRequested();
    }
}