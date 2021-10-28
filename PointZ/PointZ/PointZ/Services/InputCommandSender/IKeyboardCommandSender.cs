using System.Threading.Tasks;
using PointZ.Models.Input;
using PointZ.Models.KeyEvent;

namespace PointZ.Services.InputCommandSender
{
    public interface IKeyboardCommandSender 
    {
        public Task SendKeyboardCommandAsync(KeyboardCommand command, KeyCodeAction keyCodeAction);
        public Task SendKeyboardCommandAsync(KeyboardCommand command, string keyCode);
        public Task SendTextEntryAsync(string textEntry);
        public Task SendTextEntryAsync(char textEntry);
    }
}