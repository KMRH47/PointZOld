using System.Threading.Tasks;
using PointZ.Models.KeyEvent;
using PointZ.Models.TouchEvent;
using PointZ.Services.InputCommandSender;

namespace PointZ.Services.InputEventHandler
{
    public class InputEventHandlerService : IInputEventHandlerService
    {
        private readonly IInputEventHandler<KeyEventArgs> keyboardEventHandler;
        private readonly IInputEventHandler<TouchEventArgs> touchEventHandler;

        public InputEventHandlerService(
            IInputEventHandler<KeyEventArgs> keyboardEventHandler,
            IInputEventHandler<TouchEventArgs> touchEventHandler, IKeyboardCommandSender keyboardCommandSender)
        {
            this.keyboardEventHandler = keyboardEventHandler;
            this.touchEventHandler = touchEventHandler;
            this.KeyboardCommandSender = keyboardCommandSender;
        }

        public IKeyboardCommandSender KeyboardCommandSender { get; }

        public async Task HandleTouchEventAsync(TouchEventArgs touchEventArgs) =>
            await this.touchEventHandler.HandleAsync(touchEventArgs);

        public async Task HandleKeyEventAsync(KeyEventArgs keyEventArgs) =>
            await this.keyboardEventHandler.HandleAsync(keyEventArgs);
    }
}