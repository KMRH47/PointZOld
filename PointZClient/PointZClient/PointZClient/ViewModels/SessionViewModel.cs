using System;
using System.Diagnostics;
using System.Threading.Tasks;
using PointZClient.Models.Command;
using PointZClient.Models.DisplaySettings;
using PointZClient.Models.Server;
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
        private double previousX;
        private double previousY;
        private ServerData serverData;

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

        public override Task InitializeAsync(object parameter)
        {
            this.serverData = (ServerData) parameter;
            return Task.CompletedTask;
        }

        private async void OnScreenTouched(object sender, TouchEventArgs e)
        {
            switch (e.TouchEventAction)
            {
                case TouchEventActions.Up:
                    return;
                case TouchEventActions.Down:
                    this.previousX = e.X;
                    this.previousY = e.Y;
                    return;
            }

            DisplaySettingsData displaySettings = this.deviceUserInterfaceService.DisplaySettings;
            bool withinBounds = e.Y < displaySettings.Height - ButtonHeightPixels;
            if (!withinBounds) return;

            Debug.WriteLine($"Toucheventaction: {e.TouchEventAction}");
            Debug.WriteLine($"x: {e.X} y: {e.Y}");
            Debug.WriteLine($"previousx: {this.previousX} previousy: {this.previousY}");

            int x = (int) -(this.previousX - e.X);
            int y = (int) -(this.previousY - e.Y);
            string data = $"{x},{y}";

            await this.commandSenderService.SendAsync(MouseCommand.MoveMouseBy, data, this.serverData.Address);

            this.previousX = e.X;
            this.previousY = e.Y;
        }
    }
}