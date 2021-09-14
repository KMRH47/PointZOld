using System;
using System.Net;
using System.Threading.Tasks;
using PointZ.Models.DisplaySettings;
using PointZ.Models.Server;
using PointZ.Services.DeviceUserInterface;
using PointZ.Services.Navigation;
using PointZ.Services.SessionTouchEventHandler;
using PointZ.Services.TouchEvent;
using PointZ.ViewModels.Base;
using NavigationEventArgs = PointZ.Services.Navigation.NavigationEventArgs;

namespace PointZ.ViewModels
{
    public class SessionViewModel : ViewModelBase
    {
        private readonly ITouchEventService touchEventService;
        private readonly ISessionTouchEventHandlerService sessionTouchEventHandlerService;
        private readonly IDeviceUserInterfaceService deviceUserInterfaceService;
        private readonly IPlatformNavigationService platformNavigationService;

        private double buttonHeight;

        public SessionViewModel(
            ISessionTouchEventHandlerService sessionTouchEventHandlerService,
            ITouchEventService touchEventService,
            IDeviceUserInterfaceService deviceUserInterfaceService,
            IPlatformNavigationService platformNavigationService)
        {
            this.sessionTouchEventHandlerService = sessionTouchEventHandlerService;
            this.deviceUserInterfaceService = deviceUserInterfaceService;
            this.platformNavigationService = platformNavigationService;
            this.touchEventService = touchEventService;
            platformNavigationService.OnBackButtonPressed += OnBackButtonPressed;
            touchEventService.OnScreenTouched += OnScreenTouched;
        }

        public double ButtonHeightPixels
        {
            get => this.buttonHeight * Math.PI;
            set { this.buttonHeight = value; }
        }

        public override Task InitializeAsync(object parameter)
        {
            ServerData serverData = (ServerData)parameter;
            IPAddress ipAddress = IPAddress.Parse(serverData.Address);
            this.sessionTouchEventHandlerService.Bind(ipAddress);
            return Task.CompletedTask;
        }

        private void OnBackButtonPressed(object sender, NavigationEventArgs e)
        {
            this.touchEventService.OnScreenTouched -= OnScreenTouched;
            this.platformNavigationService.OnBackButtonPressed -= OnBackButtonPressed;
        }
      
        private async void OnScreenTouched(object sender, TouchEventArgs e)
        {
            DisplaySettingsData displaySettings = this.deviceUserInterfaceService.DisplaySettings;
            int screenHeight = displaySettings.Height;

            bool withinBounds = e.Y < screenHeight - ButtonHeightPixels;
            if (!withinBounds) return;

            await this.sessionTouchEventHandlerService.HandleAsync(e);
        }
    }
}