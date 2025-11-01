using InputSimulatorStandard;
using InputSimulatorStandard.Native;
using PointZerver.Services.VirtualKeyCodeMapper;

namespace PointZerver.Services.Simulators.Controllers
{
    public class WindowsKeyboardController : IKeyboardController
    {
        private readonly IKeyboardSimulator keyboardSimulator;
        private readonly IVirtualKeyCodeMapperService virtualKeyCodeMapperService;

        public WindowsKeyboardController(
            IKeyboardSimulator keyboardSimulator,
            IVirtualKeyCodeMapperService virtualKeyCodeMapperService)
        {
            this.keyboardSimulator = keyboardSimulator;
            this.virtualKeyCodeMapperService = virtualKeyCodeMapperService;
        }

        public void KeyPress(KeycodeAction keycodeAction)
        {
            VirtualKeyCode virtualKeyCode = this.virtualKeyCodeMapperService.MapKeycodeAction(keycodeAction);
            if (virtualKeyCode == VirtualKeyCode.NONAME) return;

            this.keyboardSimulator.KeyPress(virtualKeyCode);
        }

        public void TextEntry(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            this.keyboardSimulator.TextEntry(text);
        }
    }
}
