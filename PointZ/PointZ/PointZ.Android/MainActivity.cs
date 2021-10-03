using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using PointZ.Android.Services;
using PointZ.Android.Services.KeyEventHandler;
using PointZ.Models.PlatformEvent;
using PointZ.Services.PlatformEvent;
using PointZ.Services.PlatformInterface;
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
        private IKeyEventHandlerService keyEventHandlerService;

        public override bool DispatchTouchEvent(MotionEvent motionEventArgs)
        {
            float x = motionEventArgs.GetX();
            float y = motionEventArgs.GetY();
            TouchAction touchAction = (TouchAction)((ushort)motionEventArgs.Action);
        
            this.platformEventService.NotifyOnScreenTouched(x, y, touchAction);
        
            return base.DispatchTouchEvent(motionEventArgs);
        }

        public override bool DispatchKeyEvent(KeyEvent e)
        {
            Debug.WriteLine($"KeyEvent: {e}");
            this.keyEventHandlerService.Handle(e);
            return base.DispatchKeyEvent(e);
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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            
            base.OnCreate(savedInstanceState);

            Forms.SetTitleBarVisibility(this, AndroidTitleBarVisibility.Never);
            Forms.Init(this, savedInstanceState);

            IPlatformInterfaceService androidInterfaceService = new AndroidInterfaceService(this);
            DependencyService.RegisterSingleton(androidInterfaceService);
            this.platformEventService = DependencyService.Resolve<IPlatformEventService>();
            this.keyEventHandlerService = new KeyEventHandlerService(this.platformEventService);

            LoadApplication(new App());
        }

    }
}