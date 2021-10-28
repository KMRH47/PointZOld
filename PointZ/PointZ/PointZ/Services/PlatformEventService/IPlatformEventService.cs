using System;
using PointZ.Models.CustomEditor;
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
        public event EventHandler<KeyEventArgs> CustomEditorAction;
        public event EventHandler<KeyEventArgs> KeyEvent;
        public event EventHandler<CustomEditorEventArgs> CustomEditorSetInputType;
        void OnBackPressed();
        void OnViewDisappearing();
        void OnViewAppearing();
        void OnScreenTouched(TouchEventArgs e);
        void OnKeyEvent(KeyEventArgs e);
        void OnCustomEditorAction(KeyEventArgs e);
        void OnCustomEditorFocusRequested();
        void OnCustomEditorSetInputType(CustomEditorEventArgs e);
    }
}