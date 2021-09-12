using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using PointZ.Models.Command;
using PointZ.Services.CommandSender;
using PointZ.Services.TouchEvent;

namespace PointZ.SessionEventHandler
{
    public class SessionEventHandlerService : ISessionEventHandlerService
    {
        private readonly ICommandSenderService commandSenderService;

        private TouchEventAction previousTapEvent;

        private bool leftButtonIsPrimary = true;
        private bool doubleTapped;
        private bool tapped;

        private double scrollSpeed = 1;
        private short tapTimeFrameMs = 150;
        private short doubleTapTimeFrameMs = 250;
        private short deadZone = 10;
        private double previousX;
        private double previousY;

        private long tapTicks;

        public SessionEventHandlerService(ICommandSenderService commandSenderService)
        {
            this.commandSenderService = commandSenderService;
        }

        public void Bind(IPAddress ipAddress) => this.commandSenderService.Bind(ipAddress);

        public async Task HandleAsync(TouchEventArgs e)
        {
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
                    if (this.tapped)
                    {
                        if (DoubleTapped())
                        {
                            this.doubleTapped = true;
                            await PrimaryMouseButtonDown();
                        }
                    }

                    this.previousTapEvent = e.TouchEventAction;
                    this.previousX = e.X;
                    this.previousY = e.Y;
                    this.tapTicks = DateTime.Now.Ticks;
                    return;
                case TouchEventAction.Up:
                    switch (this.previousTapEvent)
                    {
                        case TouchEventAction.Down:
                            if (Tapped())
                            {
                                this.tapped = true;
                                await PrimaryMouseButtonClick();
                            }
                            else
                            {
                                await PrimaryMouseButtonUp();
                                this.tapped = false;
                                this.doubleTapped = false;
                            }

                            break;
                        case TouchEventAction.Pointer2Down:
                            if (Tapped())
                            {
                                await SecondaryMouseButtonUp();
                            }

                            break;
                        case TouchEventAction.Pointer3Down:
                            await this.commandSenderService.SendAsync(MouseCommand.MiddleButtonClick);
                            break;
                        case TouchEventAction.Move:
                            await PrimaryMouseButtonUp();
                            this.doubleTapped = false;
                            break;
                    }

                    break;
                case TouchEventAction.Move:
                    int x = (int)-(this.previousX - e.X);
                    int y = (int)-(this.previousY - e.Y);

                    string data;

                    int absX = Math.Abs(x);
                    int absY = Math.Abs(y);

                    switch (this.previousTapEvent)
                    {
                        case TouchEventAction.Down:


                            if (absX > this.deadZone || absY > this.deadZone)
                            {
                                x = 0;
                                y = 0;
                                this.previousTapEvent = TouchEventAction.Move;
                                goto case TouchEventAction.Move;
                            }

                            break;
                        case TouchEventAction.Pointer2Down:

                            if (absY > this.deadZone)
                            {
                                y = 0;

                                double scrollAdjustment = e.Y < 0 ? -this.scrollSpeed : this.scrollSpeed;
                                Debug.WriteLine($"y: {y}");
                                Debug.WriteLine($"scroll adjustment: {scrollAdjustment}");
                                data = scrollAdjustment.ToString(CultureInfo.InvariantCulture);
                                await this.commandSenderService.SendAsync(MouseCommand.VerticalScroll, data);
                            }

                            break;
                        case TouchEventAction.Pointer3Down:
                            break;
                        case TouchEventAction.Move:
                            data = $"{x},{y}";
                            await this.commandSenderService.SendAsync(MouseCommand.MoveMouseBy, data);

                            break;
                    }

                    break;
            }

            this.previousX = e.X;
            this.previousY = e.Y;
        }

        private bool Tapped()
        {
            if (this.doubleTapped) return false;
            long ticksElapsed = DateTime.Now.Ticks - this.tapTicks;
            double millisElapsed = TimeSpan.FromTicks(ticksElapsed).TotalMilliseconds;
            return this.tapTimeFrameMs > millisElapsed;
        }

        private bool DoubleTapped()
        {
            long ticksElapsed = DateTime.Now.Ticks - this.tapTicks;
            double millisElapsed = TimeSpan.FromTicks(ticksElapsed).TotalMilliseconds;
            return this.doubleTapTimeFrameMs > millisElapsed;
        }

        private async Task PrimaryMouseButtonClick()
        {
            if (this.leftButtonIsPrimary)
            {
                await this.commandSenderService.SendAsync(MouseCommand.LeftButtonClick);
            }
            else
            {
                await this.commandSenderService.SendAsync(MouseCommand.RightButtonClick);
            }
        }

        private async Task PrimaryMouseButtonDown()
        {
            if (this.leftButtonIsPrimary)
            {
                await this.commandSenderService.SendAsync(MouseCommand.LeftButtonDown);
            }
            else
            {
                await this.commandSenderService.SendAsync(MouseCommand.RightButtonDown);
            }
        }

        private async Task PrimaryMouseButtonUp()
        {
            if (this.leftButtonIsPrimary)
            {
                await this.commandSenderService.SendAsync(MouseCommand.LeftButtonUp);
            }
            else
            {
                await this.commandSenderService.SendAsync(MouseCommand.RightButtonUp);
            }
        }

        private async Task SecondaryMouseButtonClick()
        {
            if (this.leftButtonIsPrimary)
            {
                await this.commandSenderService.SendAsync(MouseCommand.RightButtonClick);
            }
            else
            {
                await this.commandSenderService.SendAsync(MouseCommand.LeftButtonClick);
            }
        }

        private async Task SecondaryMouseButtonDown()
        {
            if (this.leftButtonIsPrimary)
            {
                await this.commandSenderService.SendAsync(MouseCommand.RightButtonDown);
            }
            else
            {
                await this.commandSenderService.SendAsync(MouseCommand.LeftButtonDown);
            }
        }

        private async Task SecondaryMouseButtonUp()
        {
            if (this.leftButtonIsPrimary)
            {
                await this.commandSenderService.SendAsync(MouseCommand.RightButtonUp);
            }
            else
            {
                await this.commandSenderService.SendAsync(MouseCommand.LeftButtonUp);
            }
        }
    }
}