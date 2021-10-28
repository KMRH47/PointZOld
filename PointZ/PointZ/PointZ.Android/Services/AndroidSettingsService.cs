using Android.Views;
using Android.Widget;
using PointZ.Android.Extensions;
using PointZ.Services.PlatformSettings;

namespace PointZ.Android.Services
{
    public class AndroidInterfaceService : IPlatformSettingsService
    {
        private readonly MainActivity activity;

        public AndroidInterfaceService(MainActivity activity) => this.activity = activity;
        public float DisplayDensity => this.activity.GetDisplayMetrics().Density;

        public void SetSoftInputModeAdjustResize()
        {
            Window activityWindow = this.activity.Window;
            activityWindow?.SetSoftInputMode(SoftInput.AdjustResize);
        }

        public void DisplayPopupHint(string message, byte duration = 0)
        {
            Toast toast = duration == 0
                ? Toast.MakeText(this.activity, message, ToastLength.Short)
                : Toast.MakeText(this.activity, message, ToastLength.Long);
            toast?.Show();
        }
    }
}