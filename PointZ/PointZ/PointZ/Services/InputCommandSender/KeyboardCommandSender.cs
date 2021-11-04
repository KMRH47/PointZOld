using System.Net.Sockets;
using System.Threading.Tasks;
using PointZ.Models.AndroidKeyEvent;
using PointZ.Models.Input;
using PointZ.Services.Settings;

namespace PointZ.Services.InputCommandSender
{
    public class KeyboardCommandSender : InputCommandSenderBase, IKeyboardCommandSender
    {
        public KeyboardCommandSender(ISettingsService settingsService, UdpClient udpClient)
            : base(settingsService, udpClient) { }

        public async Task SendKeyboardCommandAsync(KeyboardCommand command, AndroidKeyCodeAction androidKeyCodeAction) =>
            await base.SendAsync(InputType.Keyboard, command.ToString(), androidKeyCodeAction.ToString());

        public async Task SendKeyboardCommandAsync(KeyboardCommand command, string keyCode) =>
            await base.SendAsync(InputType.Keyboard, command.ToString(), keyCode);
        
        public async Task SendTextEntryAsync(string textEntry) =>
            await base.SendAsync(InputType.Keyboard, KeyboardCommand.TextEntry.ToString(), textEntry);
        
        public async Task SendTextEntryAsync(char textEntry) =>
            await base.SendAsync(InputType.Keyboard, KeyboardCommand.TextEntry.ToString(), textEntry.ToString());
    }
}