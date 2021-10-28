using System.Diagnostics;
using System.Threading.Tasks;
using PointZ.Models.Command;
using PointZ.Models.TouchEvent;
using PointZ.Services.InputCommandSender;
using PointZ.Services.Settings;

namespace PointZ.Services.InputEventHandler
{
    public class TouchEventHandler : IInputEventHandler<TouchEventArgs>
    {
        private readonly IMouseCommandSender mouseCommandSender;
        private readonly ISettingsService settingsService;

        private TouchAction previousTapAction = TouchAction.Cancel;
        private TouchAction previousTouchAction = TouchAction.Cancel;
        private double previousX;
        private double previousY;
        private bool moving;
        private bool holdingPrimaryMouseButton;
        private bool canDoubleClick;
        private bool doubleClicked;

        public TouchEventHandler(IMouseCommandSender mouseCommandSender, ISettingsService settingsService)
        {
            this.mouseCommandSender = mouseCommandSender;
            this.settingsService = settingsService;
        }

        private bool LeftMouseButtonPrimary => this.settingsService.LeftMouseButtonPrimary;
        private byte ScrollSpeed => this.settingsService.ScrollSpeed;
        private int TapDelay => this.settingsService.TapDelayMs;
        private int DoubleTapDelay => this.settingsService.DoubleTapDelayMs;
        private int DeadZoneInitial => this.settingsService.DeadZoneInitial;
        private int DeadZoneScroll => this.settingsService.DeadZoneScroll;

        public async Task HandleAsync(TouchEventArgs e)
        {
            switch (e.TouchAction)
            {
                case TouchAction.Down:
                    if (this.canDoubleClick)
                    {
                        await PrimaryMouseButtonDownAsync();
                        this.holdingPrimaryMouseButton = true;
                    }

                    this.previousX = e.X;
                    this.previousY = e.Y;
                    this.previousTapAction = e.TouchAction;

                    break;
                case TouchAction.Pointer2Down:
                    this.previousTapAction = e.TouchAction;

                    break;
                case TouchAction.Pointer3Down:
                    this.previousTapAction = e.TouchAction;
                    break;
                case TouchAction.Up:
                    await ExecuteMouseActionAsync();
                    this.moving = false;
                    break;
                case TouchAction.Move:
                    int x = (int)-(this.previousX - e.X);
                    int y = (int)-(this.previousY - e.Y);

                    switch (this.previousTouchAction)
                    {
                        case TouchAction.Move:
                            switch (this.previousTapAction)
                            {
                                case TouchAction.Pointer2Down:
                                    int scrollDirection = y < 0 ? -ScrollSpeed : ScrollSpeed;
                                    if (DeadZoneScroll > ToAbs(x) && DeadZoneScroll > ToAbs(y)) break;

                                    await VerticalScrollAsync(scrollDirection);
                                    this.previousX = e.X;
                                    this.previousY = e.Y;
                                    break;
                                case TouchAction.Down:
                                    await this.mouseCommandSender.MoveMouseByAsync(x, y);

                                    this.previousX = e.X;
                                    this.previousY = e.Y;
                                    break;
                            }

                            break;
                        default:
                            if (DeadZoneInitial > ToAbs(x) && DeadZoneInitial > ToAbs(y)) return;

                            // Reset coordinates to avoid cursor jump
                            x = 0;
                            y = 0;
                            Debug.WriteLine($"Moving...");
                            this.moving = true;
                            goto case TouchAction.Move;
                    }

                    break;
            }

            this.previousTouchAction = e.TouchAction;
        }

        private static int ToAbs(int value) => value <= 0 ? -value : value;

        private async Task ExecuteMouseActionAsync()
        {
            switch (this.previousTapAction)
            {
                case TouchAction.Down:
                    if (this.holdingPrimaryMouseButton)
                    {
                        if (this.canDoubleClick)
                        {
                            Debug.WriteLine($"Double Clicking!");
                            this.doubleClicked = true;
                            await PrimaryMouseButtonUpAsync();
                            await PrimaryMouseButtonClickAsync();
                            await Task.Delay(TapDelay);
                            this.doubleClicked = false;
                        }
                        else
                        {
                            Debug.WriteLine($"Mouse up!");
                            await PrimaryMouseButtonUpAsync();
                        }

                        this.holdingPrimaryMouseButton = false;
                    }
                    else
                    {
                        if (this.moving) return;
                        Debug.WriteLine($"Trying Click...");
                        await NextTapIsDoubleClickAsync(DoubleTapDelay);
                        if (this.holdingPrimaryMouseButton) return;
                        if (this.doubleClicked) return;

                        Debug.WriteLine($"Clicking!");
                        await PrimaryMouseButtonClickAsync();
                    }

                    break;
                case TouchAction.PointerDown:
                    break;
                case TouchAction.Pointer2Down:
                    if (!this.moving)
                    {
                        await SecondaryMouseButtonClickAsync();
                    }

                    break;
                case TouchAction.Pointer3Down:
                    await this.mouseCommandSender.SendMouseCommandAsync(MouseCommand.MiddleButtonClick);
                    break;
            }
        }

        private async Task NextTapIsDoubleClickAsync(int timeFrameMs)
        {
            this.canDoubleClick = true;
            await Task.Delay(timeFrameMs);
            this.canDoubleClick = false;
        }

        private async Task PrimaryMouseButtonClickAsync()
        {
            await this.mouseCommandSender.SendMouseCommandAsync(
                LeftMouseButtonPrimary
                    ? MouseCommand.LeftButtonClick
                    : MouseCommand.RightButtonClick);
        }

        private async Task PrimaryMouseButtonDownAsync()
        {
            await this.mouseCommandSender.SendMouseCommandAsync(
                LeftMouseButtonPrimary
                    ? MouseCommand.LeftButtonDown
                    : MouseCommand.RightButtonDown);
        }

        private async Task PrimaryMouseButtonUpAsync()
        {
            await this.mouseCommandSender.SendMouseCommandAsync(
                LeftMouseButtonPrimary
                    ? MouseCommand.LeftButtonUp
                    : MouseCommand.RightButtonUp);
        }

        private async Task SecondaryMouseButtonClickAsync()
        {
            await this.mouseCommandSender.SendMouseCommandAsync(
                LeftMouseButtonPrimary
                    ? MouseCommand.RightButtonClick
                    : MouseCommand.LeftButtonClick);
        }

        private async Task VerticalScrollAsync(int amount) => await this.mouseCommandSender.VerticalScrollAsync(amount);
    }
}