using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using PointZ.Models.DisplaySettings;
using PointZ.Models.NavigationBar;
using PointZ.Services.DeviceUserInterface;
using PointZ.Services.Navigation;
using PointZ.Services.TouchEventService;
using Xamarin.Forms;
using Debug = System.Diagnostics.Debug;

namespace PointZ.Android
{
    [Activity(Label = "PointZ", Theme = "@style/MainTheme", Icon = "@mipmap/icon", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private ITouchEventService touchEventService;
        private IDeviceUserInterfaceService deviceUserInterfaceService;
        private IPlatformNavigationService platformNavigationService;

        public override void OnBackPressed()
        {
            this.platformNavigationService.NotifyOnBackButtonPressed();
            base.OnBackPressed();
        }

#pragma warning disable 8632
        public override bool DispatchTouchEvent(MotionEvent? motionEventArgs)
#pragma warning restore 8632
        {
            if (motionEventArgs == null)
                throw new ArgumentNullException($"{nameof(motionEventArgs)}",
                    "Error getting motion data from platform.");

            float x = motionEventArgs.GetX();
            float y = motionEventArgs.GetY();
            TouchEventAction touchEventAction = (TouchEventAction) ((ushort) motionEventArgs.Action);

            //Debug.WriteLine($"Custom Touch Event: {touchEventAction}");
            //Debug.WriteLine($"x: {x} y: {y}");
            this.touchEventService.NotifyOnScreenTouched(x, y, touchEventAction);

            return base.DispatchTouchEvent(motionEventArgs);
        }
        
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

        private void InitializePlatform()
        {
            DisplayMetrics displayMetrics = GetDisplayMetrics();

            this.touchEventService = DependencyService.Resolve<ITouchEventService>();
            this.deviceUserInterfaceService = DependencyService.Resolve<IDeviceUserInterfaceService>();
            this.platformNavigationService = DependencyService.Resolve<IPlatformNavigationService>();

            if (Resources == null) throw new Exception($"Couldn't initialize platform, {nameof(Resources)} is null.");
            int navBarHeightResId = Resources.GetIdentifier("navigation_bar_height", "dimen", "android");
            int navBarWidthResId = Resources.GetIdentifier("navigation_bar_width", "dimen", "android");
            int navBarHeightPixels = Resources.GetDimensionPixelSize(navBarHeightResId);
            int navBarWidthPixels = Resources.GetDimensionPixelSize(navBarWidthResId);

            this.deviceUserInterfaceService.NavigationBar =
                new NavigationBarData(navBarWidthPixels, navBarHeightPixels);
            this.deviceUserInterfaceService.DisplaySettings =
                new DisplaySettingsData(displayMetrics.WidthPixels, displayMetrics.HeightPixels);
        }

        private DisplayMetrics GetDisplayMetrics()
        {
            DisplayMetrics displayMetrics = new DisplayMetrics();

            if (WindowManager != null)
            {
                if (WindowManager.DefaultDisplay != null)
                {
                    WindowManager.DefaultDisplay.GetRealMetrics(displayMetrics);
                }
                else
                {
                    throw new Exception(
                        $"Couldn't resolve DisplayMetrics, {nameof(WindowManager.DefaultDisplay)} is null.");
                }
            }
            else
            {
                throw new Exception($"Couldn't resolve DisplayMetrics, {nameof(WindowManager)} is null.");
            }

            return displayMetrics;
        }
    }
}