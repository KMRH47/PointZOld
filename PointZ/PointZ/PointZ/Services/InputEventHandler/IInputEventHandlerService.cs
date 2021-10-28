using System.Threading.Tasks;
using PointZ.Models.PlatformEvent;
using PointZ.Models.TouchEvent;
using PointZ.Services.InputCommandSender;

namespace PointZ.Services.InputEventHandler
{
    public interface IInputEventHandlerService
    {
        Task HandleTouchEventAsync(TouchEventArgs touchEventArgs);
        Task HandleKeyEventAsync(KeyEventArgs keyEventArgs);
        public IKeyboardCommandSender KeyboardCommandSender { get; }
    }
}