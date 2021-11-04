using System;
using PointZ.Models.AndroidKeyEvent;
using PointZ.Models.AndroidTouchEvent;
using PointZ.Models.CustomEditor;

namespace PointZ.Services.PlatformEventService
{
    public interface IPlatformEventService
    {
        event EventHandler ViewAppearing;
        event EventHandler ViewDisappearing;
        event EventHandler BackPressed;
        event EventHandler<AndroidTouchEventArgs> TouchpadGridTouched;
        public event EventHandler<AndroidKeyEventArgs> CustomEditorAction;
        public event EventHandler<AndroidKeyEventArgs> KeyEvent;
        public event EventHandler<CustomEditorEventArgs> CustomEditorSetInputType;
        void OnBackPressed();
        void OnViewDisappearing();
        void OnViewAppearing();
        void OnTouchpadGridTouched(AndroidTouchEventArgs e);
        void OnKeyEvent(AndroidKeyEventArgs e);
        void OnCustomEditorAction(AndroidKeyEventArgs e);
        void OnCustomEditorSetInputType(CustomEditorEventArgs e);
    }
}