﻿using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using PointZClient.Models.DeviceUserInterface;
using PointZClient.Services.TouchEventService;
using Xamarin.Forms;
using Debug = System.Diagnostics.Debug;

namespace PointZClient.Android
{
    [Activity(Label = "PointZClient", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private ITouchEventService touchEventService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Forms.SetTitleBarVisibility(this, AndroidTitleBarVisibility.Never);
            Forms.Init(this, savedInstanceState);
            InitializePlatform();

            LoadApplication(new App());
        }

        public override bool DispatchTouchEvent(MotionEvent? motionEventArgs)
        {
            if (motionEventArgs == null)
                throw new ArgumentNullException($"{nameof(motionEventArgs)}",
                    "Error getting motion data from platform.");
            
            float x = motionEventArgs.GetX();
            float y = motionEventArgs.GetY();
            TouchEventActions touchEventAction = (TouchEventActions) ((ushort) motionEventArgs.Action);
            this.touchEventService.OnScreenTouched(x, y, touchEventAction);

            return base.DispatchTouchEvent(motionEventArgs);
        }

        private void InitializePlatform()
        {
            this.touchEventService = DependencyService.Resolve<ITouchEventService>();

            if (Resources == null) throw new Exception($"Couldn't initialize platform, {nameof(Resources)} is null.");
            int navBarHeightResId = Resources.GetIdentifier("navigation_bar_height", "dimen", "android");
            int navBarWidthResId = Resources.GetIdentifier("navigation_bar_width", "dimen", "android");
            int navBarHeightPixels = Resources.GetDimensionPixelSize(navBarHeightResId);
            int navBarWidthPixels = Resources.GetDimensionPixelSize(navBarWidthResId);

            NavigationBarData navigationBarData = new NavigationBarData(navBarWidthPixels, navBarHeightPixels);
            DeviceUserInterfaceData deviceUserInterfaceData = new DeviceUserInterfaceData(navigationBarData);
            Debug.WriteLine($"[Nav bar] Height: {navBarHeightPixels} Width: {navBarWidthPixels}");
        }
    }
}