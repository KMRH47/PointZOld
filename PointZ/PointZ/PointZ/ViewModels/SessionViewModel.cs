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
        private bool primaryButtonDown;

        private double tapTimeFrameMs = 150;
        private double doubleTapTimeFrameMs = 150;
        private double deadZoneSizePx = 10;
        private double buttonHeight;
        private double previousX;
        private double previousY;

        private long tapTicks;

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
            this.tapTicks = DateTime.Now.Ticks;
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
                    this.tapTicks = DateTime.Now.Ticks;
                    break;
                case TouchEventAction.Pointer2Down:
                    this.previousTapEvent = e.TouchEventAction;
                    this.tapTicks = DateTime.Now.Ticks;
                    break;
                case TouchEventAction.Down:
                    this.previousTapEvent = e.TouchEventAction;
                    this.previousX = e.X;
                    this.previousY = e.Y;
                    this.tapTicks = DateTime.Now.Ticks;
                    return;
                case TouchEventAction.Up:
                    Debug.WriteLine($"Previous Tap: {this.previousTapEvent}");
                    switch (this.previousTapEvent)
                    {
                        case TouchEventAction.Down:
                            if (Tapped())
                            {
                                Debug.WriteLine("SINGLE TAP");
                                await PrimaryButtonClick();
                            }
                            else
                            {
                                await PrimaryButtonUp();
                                this.primaryButtonDown = false;
                                this.doubleTap = false;
                            }

                            break;
                        case TouchEventAction.Pointer2Down:
                            if (Tapped())
                            {
                                await SecondaryButtonUp();
                            }

                            break;
                        case TouchEventAction.Pointer3Down:
                            await this.commandSenderService.SendAsync(MouseCommand.MiddleButtonClick);
                            break;
                        case TouchEventAction.Move:
                            await PrimaryButtonUp();
                            this.doubleTap = false;
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

                            int absX = Math.Abs(x);
                            int absY = Math.Abs(y);

                            if (DoubleTapped())
                            {
                                await PrimaryButtonDown();
                            }

                            if (absX > this.deadZoneSizePx || absY > this.deadZoneSizePx)
                            {
                                x = 0;
                                y = 0;
                                this.previousTapEvent = TouchEventAction.Move;

                                goto case TouchEventAction.Move;
                            }

                            break;
                        case TouchEventAction.Pointer2Down:
                            double scrollAdjustment = y * 0.075;
                            data = scrollAdjustment.ToString(CultureInfo.InvariantCulture);
                            await this.commandSenderService.SendAsync(MouseCommand.VerticalScroll, data);
                            break;
                        case TouchEventAction.Pointer3Down:
                            break;
                        case TouchEventAction.Move:
                            data = $"{x},{y}";
                            // Debug.WriteLine($"Moving mouse by x: {x} y: {y}");
                            await this.commandSenderService.SendAsync(MouseCommand.MoveMouseBy, data);
                            this.previousX = e.X;
                            this.previousY = e.Y;
                            break;
                    }

                    this.singleTap = false;
                    break;
            }
        }

        private async Task PrimaryButtonClick()
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


        private async Task PrimaryButtonDown()
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

        private async Task PrimaryButtonUp()
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

        private async Task SecondaryButtonClick()
        {
            Debug.WriteLine($"HOLDING PRIMARY BUTTON!");

            if (this.leftButtonIsPrimary)
            {
                await this.commandSenderService.SendAsync(MouseCommand.RightButtonClick);
            }
            else
            {
                await this.commandSenderService.SendAsync(MouseCommand.LeftButtonClick);
            }
        }


        private async Task SecondaryButtonDown()
        {
            Debug.WriteLine($"HOLDING PRIMARY BUTTON!");

            if (this.leftButtonIsPrimary)
            {
                await this.commandSenderService.SendAsync(MouseCommand.RightButtonDown);
            }
            else
            {
                await this.commandSenderService.SendAsync(MouseCommand.LeftButtonDown);
            }
        }

        private async Task SecondaryButtonUp()
        {
            Debug.WriteLine($"HOLDING PRIMARY BUTTON!");

            if (this.leftButtonIsPrimary)
            {
                await this.commandSenderService.SendAsync(MouseCommand.RightButtonUp);
            }
            else
            {
                await this.commandSenderService.SendAsync(MouseCommand.LeftButtonUp);
            }
        }


        private bool Tapped()
        {
            if (DoubleTapped()) return false;
            long ticksElapsed = DateTime.Now.Ticks - this.tapTicks;
            double millisElapsed = TimeSpan.FromTicks(ticksElapsed).TotalMilliseconds;
            Debug.WriteLine($"Time elapsed between down and up: {millisElapsed}");
            return this.tapTimeFrameMs > millisElapsed;
        }

        private bool DoubleTapped()
        {
            if (!this.doubleTap) return false;
            long ticksElapsed = DateTime.Now.Ticks - this.tapTicks;
            double millisElapsed = TimeSpan.FromTicks(ticksElapsed).TotalMilliseconds;
            return this.doubleTapTimeFrameMs > millisElapsed;
        }
    }
}