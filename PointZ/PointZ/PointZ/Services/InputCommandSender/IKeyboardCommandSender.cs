using System.Threading.Tasks;
using PointZ.Models.AndroidKeyEvent;
using PointZ.Models.Input;

namespace PointZ.Services.InputCommandSender
{
    public interface IKeyboardCommandSender 
    {
        public Task SendKeyboardCommandAsync(KeyboardCommand command, AndroidKeyCodeAction androidKeyCodeAction);
        public Task SendKeyboardCommandAsync(KeyboardCommand command, string keyCode);
        public Task SendTextEntryAsync(string textEntry);
        public Task SendTextEntryAsync(char textEntry);
    }
}