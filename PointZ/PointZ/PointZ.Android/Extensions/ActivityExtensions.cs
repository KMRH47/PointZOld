using System;
using Android.App;
using Android.Util;
using Android.Views;

namespace PointZ.Android.Extensions
{
    public static class ActivityExtensions
    {
        public static ViewGroup GetViewGroup(this Activity activity)
        {
            Window activityWindow = activity.Window;
            ViewGroup viewGroup = (ViewGroup)activityWindow?.DecorView.RootView;

            return viewGroup;
        }
        
        public static DisplayMetrics GetDisplayMetrics(this Activity activity)
        {
            DisplayMetrics displayMetrics = new();
            IWindowManager windowManager = activity.WindowManager;

            if (windowManager == null)
            {
                throw new Exception($"Couldn't resolve DisplayMetrics, {nameof(windowManager)} is null.");
            }

            if (windowManager.DefaultDisplay == null)
            {
                throw new Exception(
                    $"Couldn't resolve DisplayMetrics, {nameof(windowManager.DefaultDisplay)} is null.");
            }

            windowManager.DefaultDisplay.GetRealMetrics(displayMetrics);
            
            return displayMetrics;
        }
    }
}