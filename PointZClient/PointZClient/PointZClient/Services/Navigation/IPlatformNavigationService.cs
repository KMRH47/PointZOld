using System;

namespace PointZClient.Services.Navigation
{
    public interface IPlatformNavigationService
    {
        event EventHandler<NavigationEventArgs> OnBackButtonPressed;
        void NotifyOnBackButtonPressed();
    }
}