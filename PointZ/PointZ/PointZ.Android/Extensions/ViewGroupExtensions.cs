using Android.Views;

namespace PointZ.Android.Extensions
{
    public static class ViewGroupExtensions
    {
        public static T FindChildOfType<T>(this ViewGroup parent) where T : View
        {
            if (parent == null) return null;
            if (parent.ChildCount == 0) return null;

            for (int i = 0; i < parent.ChildCount; i++)
            {
                View child = parent.GetChildAt(i);

                if (child is T typedChild) return typedChild;
                if (child is not ViewGroup viewGroup) continue;

                T result = FindChildOfType<T>(viewGroup);
                
                if (result != null) return result;
            }

            return null;
        }
    }
}