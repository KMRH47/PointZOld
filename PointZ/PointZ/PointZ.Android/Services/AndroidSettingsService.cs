﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Android.Animation;
using Android.Content.Res;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using PointZ.Android.Extensions;
using PointZ.Models.DisplayDimensions;
using PointZ.Services.PlatformSettings;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Point = Android.Graphics.Point;
using RelativeLayout = Android.Widget.RelativeLayout;

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

            if (resources.DisplayMetrics == null)
                throw new Exception($"Couldn't initialize platform: {nameof(resources.DisplayMetrics)} is null.");
            DisplayDensity = resources.DisplayMetrics.Density;

            this.activity = activity;
        }

        public float DisplayDensity { get; set; }

        public DisplayDimensionData GetDisplayDimensions() => this.displayDimensions;

        public void ToggleKeyboard()
        {
            InputMethodManager inputMethodManager = InputMethodManager.FromContext(this.activity);
            inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.None);
        }

        public void WindowSoftInputMode(Models.SoftInput.SoftInput softInput)
        {
            ViewGroup viewGroup = activity.GetViewGroup();
            EditText editText = viewGroup.FindChildOfType<EditText>();
            //editText.ShowSoftInputOnFocus = false;
            //editText.SetRawInputType(InputTypes.Null);
            
            InputMethodManager inputMethodManager = InputMethodManager.FromContext(this.activity);
            inputMethodManager.HideSoftInputFromWindow(editText.WindowToken, 0);            
            
            SoftInput softInputAndroid = (SoftInput)softInput;
            this.activity.Window.SetSoftInputMode(softInputAndroid);
        }
    }
}