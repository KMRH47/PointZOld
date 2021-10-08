using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using PointZ.Models.Command;
using PointZ.Models.PlatformEvent;
using PointZ.Services.CommandSender;

namespace PointZ.Services.SessionEventHandler
{
    public class SessionTouchEventHandlerService : ISessionEventHandlerService<TouchEventArgs>
    {
        private readonly ITouchCommandSenderService touchCommandSenderService;

        private TouchAction previousTapAction;
        private double previousX;
        private double previousY;
        private bool leftMouseButtonIsPrimary = true;

        public SessionTouchEventHandlerService(ITouchCommandSenderService touchCommandSenderService) =>
            this.touchCommandSenderService = touchCommandSenderService;

        public void Bind(IPEndPoint ipEndPoint) => this.touchCommandSenderService.Bind(ipEndPoint);

        private async Task ExecuteTapAction()
        {
            switch (this.previousTapAction)
            {
                case TouchAction.Down:
                    await PrimaryMouseButtonClick();
                    break;
                case TouchAction.PointerDown:
                    Debug.WriteLine($"PointerDown.... What is this?");
                    break;
                case TouchAction.Pointer2Down:
                    await SecondaryMouseButtonClick();
                    break;
                case TouchAction.Pointer3Down:
                    await this.touchCommandSenderService.SendAsync(MouseCommand.MiddleButtonClick);
                    break;
            }
        }

        public async Task HandleAsync(TouchEventArgs e)
        {
            switch (e.TouchAction)
            {
                case TouchAction.Down:
                case TouchAction.Pointer2Down:
                case TouchAction.Pointer3Down:
                    Debug.WriteLine($"{e.TouchAction}");
                    this.previousX = e.X;
                    this.previousY = e.Y;
                    this.previousTapAction = e.TouchAction;
                    break;
                case TouchAction.Up:
                    Debug.WriteLine($"{e.TouchAction}");
                    await ExecuteTapAction();
                    break;
                case TouchAction.Move:
                    int x = (int)-(this.previousX - e.X);
                    int y = (int)-(this.previousY - e.Y);
                    await this.touchCommandSenderService.MoveMouseByAsync(x, y);
                    Debug.WriteLine($"Move");
                    break;
                case TouchAction.Cancel:
                    Debug.WriteLine($"Cancel");
                    break;
                case TouchAction.Outside:
                    Debug.WriteLine($"Outside");
                    break;
                case TouchAction.PointerDown:
                    Debug.WriteLine($"PointerDown");
                    break;
                case TouchAction.PointerUp:
                    Debug.WriteLine($"PointerUp");
                    break;
                case TouchAction.HoverMove:
                    Debug.WriteLine($"HoverMove");
                    break;
                case TouchAction.Scroll:
                    Debug.WriteLine($"Scroll");
                    break;
                case TouchAction.HoverEnter:
                    Debug.WriteLine($"HoverEnter");
                    break;
                case TouchAction.HoverExit:
                    Debug.WriteLine($"HoverExit");
                    break;
                case TouchAction.ButtonPress:
                    Debug.WriteLine($"ButtonPress");
                    break;
                case TouchAction.ButtonRelease:
                    Debug.WriteLine($"ButtonRelease");
                    break;
            }
        }

        private async Task PrimaryMouseButtonClick()
        {
            Debug.WriteLine($"PrimaryMouseButtonClick");
            if (this.leftMouseButtonIsPrimary)
            {
                await this.touchCommandSenderService.SendAsync(MouseCommand.LeftButtonClick);
            }
            else
            {
                await this.touchCommandSenderService.SendAsync(MouseCommand.RightButtonClick);
            }
        }

        private async Task PrimaryMouseButtonDown()
        {
            Debug.WriteLine($"PrimaryMouseButtonDown");

            if (this.leftMouseButtonIsPrimary)
            {
                await this.touchCommandSenderService.SendAsync(MouseCommand.LeftButtonDown);
            }
            else
            {
                await this.touchCommandSenderService.SendAsync(MouseCommand.RightButtonDown);
            }
        }

        private async Task PrimaryMouseButtonUp()
        {
            Debug.WriteLine($"PrimaryMouseButtonUp");

            if (this.leftMouseButtonIsPrimary)
            {
                await this.touchCommandSenderService.SendAsync(MouseCommand.LeftButtonUp);
            }
            else
            {
                await this.touchCommandSenderService.SendAsync(MouseCommand.RightButtonUp);
            }
        }

        private async Task SecondaryMouseButtonClick()
        {
            Debug.WriteLine($"SecondaryMouseButtonClick");

            if (this.leftMouseButtonIsPrimary)
            {
                await this.touchCommandSenderService.SendAsync(MouseCommand.RightButtonClick);
            }
            else
            {
                await this.touchCommandSenderService.SendAsync(MouseCommand.LeftButtonClick);
            }
        }
    }
}