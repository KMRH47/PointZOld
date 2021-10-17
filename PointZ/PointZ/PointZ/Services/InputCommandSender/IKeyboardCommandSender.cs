using System.Threading.Tasks;
using PointZ.Models.Command;

namespace PointZ.Services.InputCommandSender
{
    public interface IKeyboardCommandSender 
    {
        public Task SendKeyboardCommandAsync(KeyboardCommand command, string keyCode);
        public Task SendTextEntryAsync(string textEntry);
    }
}