using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using PointZ.Android.Renderers;
using PointZ.Android.Services;
using PointZ.Models.PlatformEvent;
using PointZ.Services.PlatformEventService;
using PointZ.Services.PlatformSettings;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Debug = System.Diagnostics.Debug;
using View = Android.Views.View;

namespace PointZ.Android
{
    [Activity(Label = "PointZ", Theme = "@style/MainTheme", Icon = "@mipmap/icon", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        private IPlatformEventService platformEventService;
        private bool customEntryFocused;

        public override View CurrentFocus => this.customEntryFocused ? null : base.CurrentFocus;

        public override bool DispatchTouchEvent(MotionEvent motionEventArgs)
        {
            float x = motionEventArgs.GetX();
            float y = motionEventArgs.GetY();
            TouchAction touchAction = (TouchAction)((ushort)motionEventArgs.Action);
        
            this.platformEventService.NotifyOnScreenTouched(x, y, touchAction);
        
            View viewInFocus = CurrentFocus;

            if (viewInFocus is null) return base.DispatchTouchEvent(motionEventArgs);
            if (viewInFocus.Parent is not CustomEditorRenderer) return base.DispatchTouchEvent(motionEventArgs);

            this.customEntryFocused = true;
            bool result = base.DispatchTouchEvent(motionEventArgs);
            this.customEntryFocused = false;
            return result;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Forms.SetTitleBarVisibility(this, AndroidTitleBarVisibility.Never);
            Forms.Init(this, savedInstanceState);

            IPlatformSettingsService androidInterfaceService = new AndroidInterfaceService(this);
            IPlatformEventService platformEventService = new AndroidEventService();
            DependencyService.RegisterSingleton(platformEventService);
            DependencyService.RegisterSingleton(androidInterfaceService);
            this.platformEventService = platformEventService;

            LoadApplication(new App());
        }

        public override void OnBackPressed()
        {
            this.platformEventService.NotifyOnBackButtonPressed();
            base.OnBackPressed();
        }

        protected override void OnPause()
        {
            this.platformEventService.NotifyOnViewDisappearing();
            base.OnPause();
        }

        protected override void OnResume()
        {
            this.platformEventService.NotifyOnViewAppearing();
            base.OnResume();
        }
    }
}