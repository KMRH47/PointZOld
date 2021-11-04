using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using PointZ.Android.Renderers;
using PointZ.Android.Services;
using PointZ.Models.AndroidKeyEvent;
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
        private bool lieAboutFocus;

        public override View CurrentFocus => this.lieAboutFocus ? null : base.CurrentFocus;

        public override bool DispatchTouchEvent(MotionEvent motionEventArgs)
        {
            View viewInFocus = CurrentFocus;
            if (viewInFocus is null) return base.DispatchTouchEvent(motionEventArgs);
            if (viewInFocus.Parent is not CustomEditorRenderer) return base.DispatchTouchEvent(motionEventArgs);
            
            this.lieAboutFocus = true;
            bool result = base.DispatchTouchEvent(motionEventArgs);
            this.lieAboutFocus = false;
            return result;
        }

        public override bool DispatchKeyEvent(KeyEvent e)
        {
            if (e.Action != KeyEventActions.Down) return base.DispatchKeyEvent(e);
            
            AndroidKeyAction androidKeyEventAction = (AndroidKeyAction)e.Action;
            AndroidKeyCodeAction androidKeyCodeAction = (AndroidKeyCodeAction)e.KeyCode;
            AndroidKeyEventArgs androidKeyEventArgs = new(androidKeyEventAction, androidKeyCodeAction);
            this.platformEventService.OnKeyEvent(androidKeyEventArgs);
            return base.DispatchKeyEvent(e);
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
            this.platformEventService.OnBackPressed();
            base.OnBackPressed();
        }

        protected override void OnPause()
        {
            this.platformEventService.OnViewDisappearing();
            base.OnPause();
        }

        protected override void OnResume()
        {
            this.platformEventService.OnViewAppearing();
            base.OnResume();
        }
    }
}