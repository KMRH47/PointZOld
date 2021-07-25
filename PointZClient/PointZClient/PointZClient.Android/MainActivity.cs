using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using PointZClient.Services.TouchEventService;
using Xamarin.Forms;

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

            Forms.Init(this, savedInstanceState);
            this.touchEventService = DependencyService.Resolve<ITouchEventService>();

            LoadApplication(new App());
        }

        public override bool DispatchTouchEvent(MotionEvent? e)
        {
            if (e != null)
            {
                float x = e.GetX();
                float y = e.GetY();
                this.touchEventService.OnScreenTouched(x, y);
            }

            return base.DispatchTouchEvent(e);
        }
    }
}