using System.Net.Sockets;
using System.Threading.Tasks;
using PointZ.Models.Command;
using PointZ.Models.PlatformEvent;
using PointZ.Services.Settings;

namespace PointZ.Services.InputCommandSender
{
    public class KeyboardCommandSender : InputCommandSenderBase, IKeyboardCommandSender
    {
        public KeyboardCommandSender(ISettingsService settingsService, UdpClient udpClient)
            : base(settingsService, udpClient) { }

        public async Task SendKeyboardCommandAsync(KeyboardCommand command, KeyCodeAction keyCodeAction) =>
            await base.SendAsync(CommandType.Keyboard, command.ToString(), keyCodeAction.ToString());

        public async Task SendKeyboardCommandAsync(KeyboardCommand command, string keyCode) =>
            await base.SendAsync(CommandType.Keyboard, command.ToString(), keyCode);
        
        public async Task SendTextEntryAsync(string textEntry) =>
            await base.SendAsync(CommandType.Keyboard, KeyboardCommand.TextEntry.ToString(), textEntry);
        
        public async Task SendTextEntryAsync(char textEntry) =>
            await base.SendAsync(CommandType.Keyboard, KeyboardCommand.TextEntry.ToString(), textEntry.ToString());
    }
}