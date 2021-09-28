using System;
using System.Diagnostics;
using Android.Content.Res;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using PointZ.Android.Extensions;
using PointZ.Models.DisplayDimensions;
using PointZ.Models.NavigationBarDimension;
using PointZ.Services.PlatformInterface;

namespace PointZ.Android.Services
{
    public class SessionAndroidInterfaceService : ISessionPlatformInterfaceService
    {
        private readonly MainActivity activity;

        private readonly NavigationBarDimensionData navigationBarDimensions;
        private readonly DisplayDimensionData displayDimensions;
        private readonly EditText editText;

        public SessionAndroidInterfaceService(MainActivity activity)
        {
            this.activity = activity;
            Resources resources = this.activity.Resources;
            if (resources == null) throw new Exception($"Couldn't initialize platform: {nameof(Resources)} is null.");
            int navBarHeightResId = resources.GetIdentifier("navigation_bar_height", "dimen", "android");
            int navBarWidthResId = resources.GetIdentifier("navigation_bar_width", "dimen", "android");
            int navBarHeightPixels = resources.GetDimensionPixelSize(navBarHeightResId);
            int navBarWidthPixels = resources.GetDimensionPixelSize(navBarWidthResId);
            DisplayMetrics displayMetrics = this.activity.GetDisplayMetrics();
            this.navigationBarDimensions = new NavigationBarDimensionData(navBarWidthPixels, navBarHeightPixels);
            this.displayDimensions = new DisplayDimensionData(displayMetrics.WidthPixels, displayMetrics.HeightPixels);

            ViewGroup viewGroup = this.activity.GetViewGroup();
            this.editText = viewGroup.FindChildOfType<EditText>();
        }
        
        public DisplayDimensionData GetDisplayDimensions() => this.displayDimensions;
        public NavigationBarDimensionData GetNavigationBarDimensions() => this.navigationBarDimensions;

        public void ToggleKeyboard()
        {
            this.editText.RequestFocus();
            InputMethodManager inputMethodManager = InputMethodManager.FromContext(this.activity);
            inputMethodManager?.ToggleSoftInput(ShowFlags.Implicit, HideSoftInputFlags.ImplicitOnly);
        }
    }
}