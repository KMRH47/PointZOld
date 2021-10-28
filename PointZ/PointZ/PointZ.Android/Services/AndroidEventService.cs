using System;
using PointZ.Models.CustomEditor;
using PointZ.Models.KeyEvent;
using PointZ.Models.TouchEvent;
using PointZ.Services.PlatformEventService;

namespace PointZ.Android.Services
{
    public class AndroidEventService : IPlatformEventService
    {
        public event EventHandler ViewAppearing;
        public event EventHandler ViewDisappearing;
        public event EventHandler BackPressed;
        public event EventHandler<TouchEventArgs> ScreenTouched;
        public event EventHandler<KeyEventArgs> CustomEditorAction;
        public event EventHandler<KeyEventArgs> KeyEvent;
        public event EventHandler<CustomEditorEventArgs> CustomEditorSetInputType;
        public event EventHandler CustomEditorFocusRequested;

        public void OnBackPressed() => this.BackPressed?.Invoke(this, EventArgs.Empty);
        public void OnViewDisappearing() => this.ViewDisappearing?.Invoke(this, EventArgs.Empty);
        public void OnViewAppearing() => this.ViewAppearing?.Invoke(this, EventArgs.Empty);
        public void OnCustomEditorAction(KeyEventArgs e) => this.CustomEditorAction?.Invoke(this, e);
        public void OnKeyEvent(KeyEventArgs e) => this.KeyEvent?.Invoke(this, e);
        public void OnScreenTouched(TouchEventArgs e) => this.ScreenTouched?.Invoke(this, e);
        public void OnCustomEditorFocusRequested() => this.CustomEditorFocusRequested?.Invoke(this, EventArgs.Empty);
        public void OnCustomEditorSetInputType(CustomEditorEventArgs e) =>
            this.CustomEditorSetInputType?.Invoke(this, e);
    }
}