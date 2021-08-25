using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using PointZClient.Models.Command;
using PointZClient.Models.DisplaySettings;
using PointZClient.Models.Server;
using PointZClient.Services.CommandSender;
using PointZClient.Services.DeviceUserInterface;
using PointZClient.Services.Navigation;
using PointZClient.Services.TouchEventService;
using PointZClient.ViewModels.Base;
using Xamarin.Forms;
using NavigationEventArgs = PointZClient.Services.Navigation.NavigationEventArgs;

namespace PointZClient.ViewModels
{
    public class SessionViewModel : ViewModelBase
    {
        private readonly ICommandSenderService commandSenderService;
        private readonly ITouchEventService touchEventService;
        private readonly IDeviceUserInterfaceService deviceUserInterfaceService;
        private readonly IPlatformNavigationService platformNavigationService;

        private double buttonHeight;
        private double previousX;
        private double previousY;

        private bool doubleTapped;
        private bool tripleTapped;
        private bool moving;

        public SessionViewModel(
            ICommandSenderService commandSenderService,
            ITouchEventService touchEventService,
            IDeviceUserInterfaceService deviceUserInterfaceService,
            IPlatformNavigationService platformNavigationService)
        {
            this.commandSenderService = commandSenderService;
            this.deviceUserInterfaceService = deviceUserInterfaceService;
            this.platformNavigationService = platformNavigationService;
            this.touchEventService = touchEventService;
            platformNavigationService.OnBackButtonPressed += OnBackButtonPressed;
            touchEventService.OnScreenTouched += OnScreenTouched;
            TapCommand = new Command(OnTap);
        }

        public ICommand TapCommand { get; }

        public double ButtonHeightPixels
        {
            get => this.buttonHeight * Math.PI;
            set { this.buttonHeight = value; }
        }

        public override Task InitializeAsync(object parameter)
        {
            ServerData serverData = (ServerData) parameter;
            IPAddress ipAddress = IPAddress.Parse(serverData.Address);
            this.commandSenderService.Bind(ipAddress);
            return Task.CompletedTask;
        }

        private void OnBackButtonPressed(object sender, NavigationEventArgs e)
        {
            this.touchEventService.OnScreenTouched -= OnScreenTouched;
            this.platformNavigationService.OnBackButtonPressed -= OnBackButtonPressed;
        }

        private async void OnTap() =>
            await this.commandSenderService.SendAsync(MouseCommand.LeftButtonClick);

        private async void OnScreenTouched(object sender, TouchEventArgs e)
        {
            DisplaySettingsData displaySettings = this.deviceUserInterfaceService.DisplaySettings;
            int screenHeight = displaySettings.Height;

            bool withinBounds = e.Y < screenHeight - ButtonHeightPixels;
            if (!withinBounds) return;

            switch (e.TouchEventAction)
            {
                case TouchEventAction.Pointer3Down:
                    this.doubleTapped = false;
                    this.tripleTapped = true;
                    break;
                case TouchEventAction.Pointer2Down:
                    this.doubleTapped = true;
                    break;
                case TouchEventAction.Down:
                    this.previousX = e.X;
                    this.previousY = e.Y;
                    return;
                case TouchEventAction.Up:
                    if (this.doubleTapped && !this.tripleTapped)
                    {
                        this.doubleTapped = false;
                        if (this.moving) break;
                        await this.commandSenderService.SendAsync(MouseCommand.RightButtonClick);
                    }
                    else if (this.tripleTapped)
                    {
                        this.tripleTapped = false;
                        await this.commandSenderService.SendAsync(MouseCommand.MiddleButtonClick);
                    }
                    this.moving = false;
                    return;
                case TouchEventAction.Move:
                    int x = (int) -(this.previousX - e.X);
                    int y = (int) -(this.previousY - e.Y);
                    string data;

                    if (this.doubleTapped)
                    {
                        this.moving = true;
                        data = (y * 0.2).ToString(CultureInfo.InvariantCulture);
                        await this.commandSenderService.SendAsync(MouseCommand.VerticalScroll, data);
                    }
                    else if (this.tripleTapped)
                    {
                    }
                    else
                    {
                        data = $"{x},{y}";
                        await this.commandSenderService.SendAsync(MouseCommand.MoveMouseBy, data);
                    }

                    this.previousX = e.X;
                    this.previousY = e.Y;
                    break;
            }
        }
    }
}