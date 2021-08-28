using System;
using PointZ.Services.Navigation;
using Xamarin.Forms;
using NavigationEventArgs = PointZ.Services.Navigation.NavigationEventArgs;

[assembly: Dependency(typeof(PointZ.Android.Services.PlatformNavigationService))]

namespace PointZ.Android.Services
{
    public class PlatformNavigationService : IPlatformNavigationService
    {
        public event EventHandler<NavigationEventArgs> OnBackButtonPressed;
        
        public void NotifyOnBackButtonPressed()
        {
            NavigationEventArgs e = new NavigationEventArgs();
            OnBackButtonPressed?.Invoke(this, e);   
        }
    }
}