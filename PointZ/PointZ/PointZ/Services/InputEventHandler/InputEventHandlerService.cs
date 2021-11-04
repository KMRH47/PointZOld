using System.Threading.Tasks;
using PointZ.Models.AndroidKeyEvent;
using PointZ.Models.AndroidTouchEvent;
using PointZ.Services.InputCommandSender;

namespace PointZ.Services.InputEventHandler
{
    public class InputEventHandlerService : IInputEventHandlerService
    {
        private readonly IInputEventHandler<AndroidKeyEventArgs> keyboardEventHandler;
        private readonly IInputEventHandler<AndroidTouchEventArgs> touchEventHandler;

        public InputEventHandlerService(
            IInputEventHandler<AndroidKeyEventArgs> keyboardEventHandler,
            IInputEventHandler<AndroidTouchEventArgs> touchEventHandler, IKeyboardCommandSender keyboardCommandSender)
        {
            this.keyboardEventHandler = keyboardEventHandler;
            this.touchEventHandler = touchEventHandler;
            this.KeyboardCommandSender = keyboardCommandSender;
        }

        public IKeyboardCommandSender KeyboardCommandSender { get; }

        public async Task HandleTouchEventAsync(AndroidTouchEventArgs androidTouchEventArgs) =>
            await this.touchEventHandler.HandleAsync(androidTouchEventArgs);

        public async Task HandleKeyEventAsync(AndroidKeyEventArgs androidKeyEventArgs) =>
            await this.keyboardEventHandler.HandleAsync(androidKeyEventArgs);
    }
}