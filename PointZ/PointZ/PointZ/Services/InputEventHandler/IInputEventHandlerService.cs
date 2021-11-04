using System.Threading.Tasks;
using PointZ.Models.AndroidKeyEvent;
using PointZ.Models.AndroidTouchEvent;
using PointZ.Services.InputCommandSender;

namespace PointZ.Services.InputEventHandler
{
    public interface IInputEventHandlerService
    {
        Task HandleTouchEventAsync(AndroidTouchEventArgs androidTouchEventArgs);
        Task HandleKeyEventAsync(AndroidKeyEventArgs androidKeyEventArgs);
        public IKeyboardCommandSender KeyboardCommandSender { get; }
    }
}