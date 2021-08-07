using System;
using System.Diagnostics;
using PointZClient.Models.CursorBehavior;
using PointZClient.Models.DisplaySettings;
using PointZClient.Services.CommandSender;
using PointZClient.Services.DeviceUserInterface;
using PointZClient.Services.TouchEventService;
using PointZClient.ViewModels.Base;

namespace PointZClient.ViewModels
{
    public class SessionViewModel : ViewModelBase
    {
        private readonly ICommandSenderService commandSenderService;
        private readonly IDeviceUserInterfaceService deviceUserInterfaceService;
        private double buttonHeight;

        public SessionViewModel(ICommandSenderService commandSenderService, ITouchEventService touchEventService,
            IDeviceUserInterfaceService deviceUserInterfaceService)
        {
            this.commandSenderService = commandSenderService;
            this.deviceUserInterfaceService = deviceUserInterfaceService;
            touchEventService.ScreenTouched += OnScreenTouched;
        }

        public double ButtonHeightPixels
        {
            get => this.buttonHeight * Math.PI;
            set
            {
                this.buttonHeight = value;
                Debug.WriteLine($"ButtonHeightPixels: {value}");
            }
        }

        private void OnScreenTouched(object sender, TouchEventArgs e)
        {
            DisplaySettingsData displaySettings = this.deviceUserInterfaceService.DisplaySettings;

            bool withinBounds = e.Y < displaySettings.Height - ButtonHeightPixels;
            if (!withinBounds) return;

            float x = e.X;
            float y = e.Y;

            CursorBehavior cursorBehavior = new(x, y);
            this.commandSenderService.Send(cursorBehavior);

            Debug.WriteLine($"Touch at X: {x} Y: {y}");
        }
    }
}