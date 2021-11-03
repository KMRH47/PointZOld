using System.Diagnostics;
using Xamarin.Forms;

namespace PointZ.Behaviors
{
    public static class FocusBehavior
    {
        public static bool GetIsFocused(BindableObject obj) => (bool)obj.GetValue(IsFocusedProperty);
        public static void SetIsFocused(BindableObject obj, bool value) => obj.SetValue(IsFocusedProperty, value);

        public static readonly BindableProperty IsFocusedProperty =
            BindableProperty.CreateAttached("IsFocused", typeof(bool), typeof(FocusBehavior), false,
                propertyChanged: OnIsFocusedPropertyChanged);

        private static void OnIsFocusedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not View view) return;

            bool isFocused = (bool)newValue;

            if (isFocused)
            {
                view.Focus();
            }
        }
    }
}