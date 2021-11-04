using System;
using PointZ.Models.AndroidKeyEvent;
using PointZ.Models.AndroidTouchEvent;
using PointZ.Models.CustomEditor;
using PointZ.Services.PlatformEventService;

namespace PointZ.Android.Services
{
    public class AndroidEventService : IPlatformEventService
    {
        public event EventHandler ViewAppearing;
        public event EventHandler ViewDisappearing;
        public event EventHandler BackPressed;
        public event EventHandler<AndroidTouchEventArgs> TouchpadGridTouched;
        public event EventHandler<AndroidKeyEventArgs> KeyEvent;
        public event EventHandler<AndroidKeyEventArgs> CustomEditorAction;
        public event EventHandler<CustomEditorEventArgs> CustomEditorSetInputType;

        public void OnBackPressed() => this.BackPressed?.Invoke(this, EventArgs.Empty);
        public void OnViewDisappearing() => this.ViewDisappearing?.Invoke(this, EventArgs.Empty);
        public void OnViewAppearing() => this.ViewAppearing?.Invoke(this, EventArgs.Empty);
        public void OnCustomEditorAction(AndroidKeyEventArgs e) => this.CustomEditorAction?.Invoke(this, e);
        public void OnKeyEvent(AndroidKeyEventArgs e) => this.KeyEvent?.Invoke(this, e);
        public void OnTouchpadGridTouched(AndroidTouchEventArgs e) => this.TouchpadGridTouched?.Invoke(this, e);
        public void OnCustomEditorSetInputType(CustomEditorEventArgs e) =>
            this.CustomEditorSetInputType?.Invoke(this, e);
    }
}