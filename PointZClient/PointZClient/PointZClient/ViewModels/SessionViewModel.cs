using System;
using System.Diagnostics;
using PointZClient.Services.CommandSender;
using PointZClient.Services.TouchEventService;
using PointZClient.ViewModels.Base;

namespace PointZClient.ViewModels
{
    public class SessionViewModel : ViewModelBase
    {
        private readonly ICommandSenderService commandSenderService;
        private double stackLayoutHeight;
        private double buttonHeight;

        public SessionViewModel(ICommandSenderService commandSenderService, ITouchEventService touchEventService)
        {
            this.commandSenderService = commandSenderService;
            touchEventService.ScreenTouched += OnScreenTouched;
        }

        public double StackLayoutHeightPixels
        {
            get => this.stackLayoutHeight * Math.PI;
            set
            {
            this.stackLayoutHeight = value;
            Debug.WriteLine($"StackLayoutHeight: {value}");
            }
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
            // if screen height - buttonheight then
            bool withinBounds = e.Y < StackLayoutHeightPixels  - ButtonHeightPixels;

            if (!withinBounds) return;
            float x = e.X;
            float y = e.Y;

            Debug.WriteLine($"Touch at X: {x} Y: {y}");
            Debug.WriteLine($"StackLayoutHeightPixels {StackLayoutHeightPixels}");
            Debug.WriteLine($"StackLayoutHeightPixels - ButtonHeightPixels {StackLayoutHeightPixels - ButtonHeightPixels}");
            Debug.WriteLine($"StackLayoutHeight: {StackLayoutHeightPixels}");
            Debug.WriteLine($"ButtonHeight: {ButtonHeightPixels}");
            
            //Debug.WriteLine($"Touchpad: Height: {TouchpadHeight} Width: {TouchpadWidth}");
        }
    }
}