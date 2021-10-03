using System;
using System.Diagnostics;
using Android.Content.Res;
using Android.Graphics;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using PointZ.Android.Extensions;
using PointZ.Models.DisplayDimensions;
using PointZ.Services.PlatformInterface;

namespace PointZ.Android.Services
{
    public class AndroidInterfaceService : IPlatformInterfaceService
    {
        private readonly MainActivity activity;
        private readonly DisplayDimensionData displayDimensions;
        private readonly EditText editText;

        public AndroidInterfaceService(MainActivity activity)
        {
            Resources resources = activity.Resources;
            if (resources == null) throw new Exception($"Couldn't initialize platform: {nameof(Resources)} is null.");
            int navBarHeightResId = resources.GetIdentifier("navigation_bar_height", "dimen", "android");
            int navBarWidthResId = resources.GetIdentifier("navigation_bar_width", "dimen", "android");
            float navBarHeightPixels = resources.GetDimensionPixelSize(navBarHeightResId);
            float navBarWidthPixels = resources.GetDimensionPixelSize(navBarWidthResId);

            IWindowManager windowManager = activity.WindowManager;
            if (windowManager == null)
                throw new Exception($"Couldn't initialize platform: {nameof(IWindowManager)} is null.");
            
            Display display = windowManager.DefaultDisplay;
            if (display == null) throw new Exception($"Couldn't initialize platform: {nameof(Display)} is null.");

            Point sizeSmall = new(), sizeLarge = new();
            display.GetCurrentSizeRange(sizeSmall, sizeLarge);
            this.displayDimensions = new DisplayDimensionData(sizeSmall.X, sizeLarge.Y);

            ViewGroup viewGroup = activity.GetViewGroup();
            this.editText = viewGroup.FindChildOfType<EditText>();

            if (resources.DisplayMetrics == null) throw new Exception($"Couldn't initialize platform: {nameof(resources.DisplayMetrics)} is null.");
            DisplayDensity = resources.DisplayMetrics.Density;
            this.activity = activity;
        }

        public float DisplayDensity { get; set; }

        public DisplayDimensionData GetDisplayDimensions() => this.displayDimensions;

        public void ToggleKeyboard()
        {
            this.editText.RequestFocus();
            InputMethodManager inputMethodManager = InputMethodManager.FromContext(this.activity);
            inputMethodManager?.ToggleSoftInput(ShowFlags.Implicit, HideSoftInputFlags.ImplicitOnly);
        }
    }
}