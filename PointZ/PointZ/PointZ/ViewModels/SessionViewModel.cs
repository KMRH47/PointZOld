using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using PointZ.Models.Command;
using PointZ.Models.DisplaySettings;
using PointZ.Models.Server;
using PointZ.Services.CommandSender;
using PointZ.Services.DeviceUserInterface;
using PointZ.Services.Navigation;
using PointZ.Services.TouchEventService;
using PointZ.ViewModels.Base;
using Xamarin.Forms;
using NavigationEventArgs = PointZ.Services.Navigation.NavigationEventArgs;

namespace PointZ.ViewModels
{
    public class SessionViewModel : ViewModelBase
    {
        private readonly ICommandSenderService commandSenderService;
        private readonly ITouchEventService touchEventService;
        private readonly IDeviceUserInterfaceService deviceUserInterfaceService;
        private readonly IPlatformNavigationService platformNavigationService;

        private TouchEventAction previousTapEvent;

        private bool leftButtonIsPrimary = true;
        private bool singleTap;
        private bool doubleTap;

        private double doubleTapTimeFrameMs = 250;
        private long doubleTapTick;

        private double buttonHeight;
        private double previousX;
        private double previousY;

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
            SingleTapCommand = new Command(OnSingleTap);
            DoubleTapCommand = new Command(OnDoubleTap);
        }

        public ICommand SingleTapCommand { get; }
        public ICommand DoubleTapCommand { get; }

        public double ButtonHeightPixels
        {
            get => this.buttonHeight * Math.PI;
            set { this.buttonHeight = value; }
        }

        public override Task InitializeAsync(object parameter)
        {
            ServerData serverData = (ServerData)parameter;
            IPAddress ipAddress = IPAddress.Parse(serverData.Address);
            this.commandSenderService.Bind(ipAddress);
            return Task.CompletedTask;
        }

        private void OnBackButtonPressed(object sender, NavigationEventArgs e)
        {
            this.touchEventService.OnScreenTouched -= OnScreenTouched;
            this.platformNavigationService.OnBackButtonPressed -= OnBackButtonPressed;
        }

        private void OnSingleTap() => this.singleTap = true;

        private void OnDoubleTap()
        {
            this.doubleTap = true;
            this.doubleTapTick = DateTime.Now.Ticks;
        }

        private async void OnScreenTouched(object sender, TouchEventArgs e)
        {
            DisplaySettingsData displaySettings = this.deviceUserInterfaceService.DisplaySettings;
            int screenHeight = displaySettings.Height;

            bool withinBounds = e.Y < screenHeight - ButtonHeightPixels;
            if (!withinBounds) return;

            switch (e.TouchEventAction)
            {
                case TouchEventAction.Pointer3Down:
                    this.previousTapEvent = e.TouchEventAction;
                    break;
                case TouchEventAction.Pointer2Down:
                    this.previousTapEvent = e.TouchEventAction;
                    break;
                case TouchEventAction.Down:
                    this.previousTapEvent = e.TouchEventAction;
                    this.previousX = e.X;
                    this.previousY = e.Y;
                    return;
                case TouchEventAction.Up:
                    switch (this.previousTapEvent)
                    {
                        case TouchEventAction.Down:
                            if (DoubleTapped())
                            {
                                Debug.WriteLine($"Double tapped = true");
                                await ClickPrimaryButton();
                            }
                            else break;

                            Debug.WriteLine($"Double tapped = false");

                            await ClickPrimaryButton();
                            await ReleasePrimaryButton();

                            break;
                        case TouchEventAction.Pointer2Down:
                            await this.commandSenderService.SendAsync(MouseCommand.RightButtonClick);
                            break;
                        case TouchEventAction.Pointer3Down:
                            await this.commandSenderService.SendAsync(MouseCommand.MiddleButtonClick);
                            break;
                        case TouchEventAction.Move:
                            break;
                    }
                    break;
                case TouchEventAction.Move:
                    int x = (int)-(this.previousX - e.X);
                    int y = (int)-(this.previousY - e.Y);
                    string data;

                    switch (this.previousTapEvent)
                    {
                        case TouchEventAction.Down:

                            if (DoubleTapped())
                            {
                                Debug.WriteLine($"DOUBLE TAPPED TRUE???????");
                                await HoldPrimaryButton();
                            }

                            this.previousTapEvent = TouchEventAction.Move;
                            goto case TouchEventAction.Move;
                        case TouchEventAction.Pointer2Down:
                            double scrollAdjustment = y * 0.2;
                            data = scrollAdjustment < 1
                                ? 1.ToString()
                                : scrollAdjustment.ToString(CultureInfo.InvariantCulture);
                            await this.commandSenderService.SendAsync(MouseCommand.VerticalScroll, data);
                            break;
                        case TouchEventAction.Pointer3Down:
                            break;
                        case TouchEventAction.Move:
                            data = $"{x},{y}";
                            await this.commandSenderService.SendAsync(MouseCommand.MoveMouseBy, data);
                            break;
                    }

                    this.previousX = e.X;
                    this.previousY = e.Y;
                    this.singleTap = false;
                    this.doubleTap = false;
                    break;
            }
        }

        private async Task ClickPrimaryButton()
        {
            Debug.WriteLine($"CLICKING PRIMARY BUTTON!");

            if (this.leftButtonIsPrimary)
            {
                await this.commandSenderService.SendAsync(MouseCommand.LeftButtonClick);
            }
            else
            {
                await this.commandSenderService.SendAsync(MouseCommand.RightButtonClick);
            }
        }

        private async Task HoldPrimaryButton()
        {
            Debug.WriteLine($"HOLDING PRIMARY BUTTON!");

            if (this.leftButtonIsPrimary)
            {
                await this.commandSenderService.SendAsync(MouseCommand.LeftButtonDown);
            }
            else
            {
                await this.commandSenderService.SendAsync(MouseCommand.RightButtonDown);
            }
        }

        private async Task ReleasePrimaryButton()
        {
            Debug.WriteLine($"RELEASING PRIMARY BUTTON!");

            if (this.leftButtonIsPrimary)
            {
                await this.commandSenderService.SendAsync(MouseCommand.LeftButtonUp);
            }
            else
            {
                await this.commandSenderService.SendAsync(MouseCommand.RightButtonUp);
            }
        }

        private bool DoubleTapped()
        {
            if (!this.doubleTap) return false;
            long ticksElapsed = DateTime.Now.Ticks - this.doubleTapTick;
            double millisElapsed = TimeSpan.FromMilliseconds(ticksElapsed).TotalMilliseconds;
            return this.doubleTapTimeFrameMs > millisElapsed;
        }
    }
}