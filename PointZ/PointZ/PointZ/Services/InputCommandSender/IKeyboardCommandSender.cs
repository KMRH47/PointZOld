using System.Threading.Tasks;
using PointZ.Models.Command;
using PointZ.Models.PlatformEvent;

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