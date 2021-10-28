using System;
using PointZ.Models.KeyEvent;
using PointZ.Models.TouchEvent;

namespace PointZ.Services.PlatformEventService
{
    public interface IPlatformEventService
    {
        event EventHandler ViewAppearing;
        event EventHandler ViewDisappearing;
        event EventHandler BackPressed;
        event EventHandler CustomEditorFocusRequested;
        event EventHandler<TouchEventArgs> ScreenTouched;
        public event EventHandler<KeyEventArgs> CustomEditorBackPressed;
        public event EventHandler<KeyEventArgs> KeyEvent;
        public event EventHandler CustomEditorInputModeChanged;
        void OnBackPressed();
        void OnViewDisappearing();
        void OnViewAppearing();
        void OnScreenTouched(TouchEventArgs e);
        void OnKeyEvent(KeyEventArgs e);
        void OnCustomEditorBackPressed(KeyEventArgs e);
        void OnCustomEditorFocusRequested();
        void OnCustomEditorInputModeChanged();
    }
}