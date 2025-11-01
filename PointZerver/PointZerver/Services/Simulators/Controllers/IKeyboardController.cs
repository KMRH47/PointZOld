using PointZerver.Services.VirtualKeyCodeMapper;

namespace PointZerver.Services.Simulators.Controllers
{
    public interface IKeyboardController
    {
        void KeyPress(KeycodeAction keycodeAction);
        void TextEntry(string text);
    }
}
