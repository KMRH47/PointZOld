using System;
using PointZClient.Services.Navigation;
using Xamarin.Forms;
using NavigationEventArgs = PointZClient.Services.Navigation.NavigationEventArgs;

[assembly: Dependency(typeof(PointZClient.Android.Services.PlatformNavigationService))]

namespace PointZClient.Android.Services
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