using System;
using System.Threading.Tasks;
using PointZ.Models.Command;
using PointZ.Models.PlatformEvent;
using PointZ.Services.InputCommandSender;

namespace PointZ.Services.InputEventHandler
{
    public class KeyboardEventHandler : IInputCommandSender<KeyEventArgs>
    {
        private readonly IKeyboardCommandSender keyboardCommandSender;

        public KeyboardEventHandler(IKeyboardCommandSender keyboardCommandSender)
        {
            this.keyboardCommandSender = keyboardCommandSender;
        }

        public async Task HandleAsync(KeyEventArgs e)
        {
            switch (e.KeyAction)
            {
                case KeyAction.Up:
                    break;
                case KeyAction.Multiple:
                case KeyAction.Down:
                    await this.keyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown, e.KeyCode);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}