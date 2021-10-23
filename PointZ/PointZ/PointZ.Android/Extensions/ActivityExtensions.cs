using System;
using Android.App;
using Android.Graphics;
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

        public static int[] GetCurrentDisplaySize(this Activity activity)
        {
            IWindowManager windowManager = activity.WindowManager;
            if (windowManager == null)
                throw new Exception($"Couldn't initialize platform: {nameof(IWindowManager)} is null.");

            Display display = windowManager.DefaultDisplay;
            if (display == null) throw new Exception($"Couldn't initialize platform: {nameof(Display)} is null.");

            Point sizeSmall = new(), sizeLarge = new();
            display.GetCurrentSizeRange(sizeSmall, sizeLarge);

            return new[]{ sizeSmall.X, sizeLarge.Y };
        }
    }
}