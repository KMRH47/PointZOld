﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using PointZ.Models.Command;
using PointZ.Models.PlatformEvent;
using PointZ.Services.CommandSender;

namespace PointZ.Services.SessionEventHandler
{
    public class SessionTouchEventHandlerService : ISessionEventHandlerService<TouchEventArgs>
    {
        private readonly ICommandSenderService commandSenderService;

        private TouchAction previousTapAction;
        private CancellationTokenSource tapCancellationTokenSource;

        private bool leftMouseButtonIsPrimary = true;
        private bool doubleTapped;
        private bool tapped;
        private bool secondaryMouseButtonClicked;
        private bool cancelledTap;

        private double scrollSpeed = 1;
        private short tapTimeFrameMs = 200;
        private short deadZoneMove = 10;
        private short deadZoneTap = 70;
        private double previousX;
        private double previousY;


        private long tapTicks;

        public SessionTouchEventHandlerService(ICommandSenderService commandSenderService)
        {
            this.commandSenderService = commandSenderService;
            this.tapCancellationTokenSource = new CancellationTokenSource();
        }

        public void Bind(IPAddress ipAddress) => this.commandSenderService.Bind(ipAddress);

        public async Task HandleAsync(TouchEventArgs e)
        {
            CancellationToken token = this.tapCancellationTokenSource.Token;
            int x = (int)-(this.previousX - e.X);
            int y = (int)-(this.previousY - e.Y);

            switch (e.TouchAction)
            {
                case TouchAction.Pointer3Down:
                    this.previousTapAction = e.TouchAction;
                    this.tapTicks = DateTime.Now.Ticks;
                    break;
                case TouchAction.Pointer2Down:
                    this.tapCancellationTokenSource.Cancel();
                    this.secondaryMouseButtonClicked = true;
                    this.previousTapAction = e.TouchAction;
                    this.tapTicks = DateTime.Now.Ticks;
                    break;
                case TouchAction.Down:
                    _ = this.tapped ? TryHold() : TryTap(this.tapTimeFrameMs, token);
                    this.previousTapAction = e.TouchAction;
                    this.previousX = e.X;
                    this.previousY = e.Y;

                    return;
                case TouchAction.Up:

                    if (token.IsCancellationRequested)
                        this.tapCancellationTokenSource = new CancellationTokenSource();

                    switch (this.previousTapAction)
                    {
                        case TouchAction.Down:
                            Debug.WriteLine($"Up -> Down");

                            if (token.IsCancellationRequested)
                            {
                                await PrimaryMouseButtonClick();
                            }
                            else
                            {
                                await PrimaryMouseButtonUp();
                            }

                            break;
                        case TouchAction.Pointer2Down:
                            Debug.WriteLine($"Up -> Pointer2Down");

                                await SecondaryMouseButtonClick();

                            break;
                        case TouchAction.Pointer3Down:
                            await this.commandSenderService.SendAsync(MouseCommand.MiddleButtonClick);
                            break;
                        case TouchAction.Move:
                            Debug.WriteLine($"Up -> Move");

                            if (!this.secondaryMouseButtonClicked)
                            {
                                await PrimaryMouseButtonUp();
                            }

                            this.doubleTapped = false;
                            break;
                    }

                    break;
                case TouchAction.Move:

                    string data;

                    switch (this.previousTapAction)
                    {
                        case TouchAction.Down:
                            Debug.WriteLine($"Move -> Down");

                            this.tapCancellationTokenSource.Cancel();

                            if (ValueOutsideDeadzoneMove(x) || ValueOutsideDeadzoneMove(y))
                            {
                                x = 0;
                                y = 0;
                                this.previousTapAction = TouchAction.Move;
                                goto case TouchAction.Move;
                            }

                            break;
                        case TouchAction.Pointer2Down:
                            Debug.WriteLine($"Move -> Pointer2Down");

                            if (y == 0) break;
                            double scrollAdjustment = y < 0 ? -this.scrollSpeed : this.scrollSpeed;
                            Debug.WriteLine($"y: {y}");
                            Debug.WriteLine($"scroll adjustment: {scrollAdjustment}");
                            data = scrollAdjustment.ToString(CultureInfo.InvariantCulture);
                            await this.commandSenderService.SendAsync(MouseCommand.VerticalScroll, data);

                            this.previousX = e.X;
                            this.previousY = e.Y;

                            break;
                        case TouchAction.Pointer3Down:
                            break;
                        case TouchAction.Move:

                            Debug.WriteLine($"Move -> Move");

                            data = $"{x},{y}";
                            await this.commandSenderService.SendAsync(MouseCommand.MoveMouseBy, data);
                            this.previousX = e.X;
                            this.previousY = e.Y;
                            break;
                    }

                    break;
            }
        }

        private async Task TryTap(int delayMs, CancellationToken token)
        {
            try
            {
                this.tapped = true;
                await Task.Delay(delayMs, token);
                await PrimaryMouseButtonClick();
            }
            finally
            {
                this.tapped = false;
            }
        }

        private async Task TryHold()
        {
            this.tapCancellationTokenSource.Cancel();
            this.cancelledTap = true;
            await PrimaryMouseButtonDown();
            await Task.Delay(150);
            this.cancelledTap = false;
        }

        private bool ValueOutsideDeadzoneMove(int value) => Math.Abs(value) > this.deadZoneMove;
        private bool ValueWithinDeadzoneTap(int value) => Math.Abs(value) < this.deadZoneTap;

        private async Task PrimaryMouseButtonClick()
        {
            Debug.WriteLine($"PrimaryMouseButtonClick");
            this.secondaryMouseButtonClicked = false;
            if (this.leftMouseButtonIsPrimary)
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
            Debug.WriteLine($"PrimaryMouseButtonDown");

            if (this.leftMouseButtonIsPrimary)
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
            Debug.WriteLine($"PrimaryMouseButtonUp");

            if (this.leftMouseButtonIsPrimary)
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
            Debug.WriteLine($"SecondaryMouseButtonClick");

            if (this.leftMouseButtonIsPrimary)
            {
                await this.commandSenderService.SendAsync(MouseCommand.RightButtonClick);
            }
            else
            {
                await this.commandSenderService.SendAsync(MouseCommand.LeftButtonClick);
            }
        }
    }
}