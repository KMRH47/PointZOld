using System;

namespace PointZ.Services.Navigation
{
    public interface IPlatformNavigationService
    {
        event EventHandler<NavigationEventArgs> OnBackButtonPressed;
        void NotifyOnBackButtonPressed();
    }
}