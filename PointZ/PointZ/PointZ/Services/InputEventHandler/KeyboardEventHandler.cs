using System;
using System.Threading.Tasks;
using PointZ.Models.AndroidKeyEvent;
using PointZ.Models.Input;
using PointZ.Services.InputCommandSender;

namespace PointZ.Services.InputEventHandler
{
    public class KeyboardEventHandler : IInputEventHandler<AndroidKeyEventArgs>
    {
        private readonly IKeyboardCommandSender keyboardCommandSender;

        public KeyboardEventHandler(IKeyboardCommandSender keyboardCommandSender)
        {
            this.keyboardCommandSender = keyboardCommandSender;
        }

        public async Task HandleAsync(AndroidKeyEventArgs e)
        {
            switch (e.AndroidKeyAction)
            {
                case AndroidKeyAction.Up:
                    break;
                case AndroidKeyAction.Multiple:
                case AndroidKeyAction.Down:
                    await this.keyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyPress, e.AndroidKeyCodeAction);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}