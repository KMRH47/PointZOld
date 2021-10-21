using System;
using Android.Content.Res;
using Android.Views;
using Android.Widget;
using PointZ.Android.Extensions;
using PointZ.Models.DisplayDimensions;
using PointZ.Services.PlatformSettings;
using Xamarin.Forms.Platform.Android;
using Point = Android.Graphics.Point;

namespace PointZ.Android.Services
{
    public class AndroidInterfaceService : IPlatformSettingsService
    {
        private readonly MainActivity activity;
        private readonly DisplayDimensionData displayDimensions;

        public AndroidInterfaceService(MainActivity activity)
        {
            Resources resources = activity.Resources;
            if (resources == null) throw new Exception($"Couldn't initialize platform: {nameof(Resources)} is null.");

            IWindowManager windowManager = activity.WindowManager;
            if (windowManager == null)
                throw new Exception($"Couldn't initialize platform: {nameof(IWindowManager)} is null.");

            Display display = windowManager.DefaultDisplay;
            if (display == null) throw new Exception($"Couldn't initialize platform: {nameof(Display)} is null.");

            Point sizeSmall = new(), sizeLarge = new();
            display.GetCurrentSizeRange(sizeSmall, sizeLarge);
            this.displayDimensions = new DisplayDimensionData(sizeSmall.X, sizeLarge.Y);

            if (resources.DisplayMetrics == null)
                throw new Exception($"Couldn't initialize platform: {nameof(resources.DisplayMetrics)} is null.");
            DisplayDensity = resources.DisplayMetrics.Density;

            this.activity = activity;
        }

        public float DisplayDensity { get; set; }

        public DisplayDimensionData GetDisplayDimensions() => this.displayDimensions;

        public void ToggleKeyboard()
        {
            //InputMethodManager inputMethodManager = InputMethodManager.FromContext(this.activity);
            //inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.None);
        }

        /// <summary>
        /// Primarily for the soft input not to push the current view up... 
        /// </summary>
        /// <param name="softInput"></param>
        public void WindowSoftInputMode(Models.SoftInput.SoftInput softInput)
        {
            SoftInput softInputAndroid = (SoftInput)softInput;
            this.activity.Window.SetSoftInputMode(softInputAndroid);

            ViewGroup viewGroup = this.activity.GetViewGroup();
            EditText editText = viewGroup.FindChildOfType<EditText>();
            
            editText.SetCursorVisible(false);
            editText.SetTextIsSelectable(false);
            editText.SetBackground(null);
        }
    }
}