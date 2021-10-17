using System.Net.Sockets;
using System.Threading.Tasks;
using PointZ.Models.Command;
using PointZ.Services.Settings;

namespace PointZ.Services.InputCommandSender
{
    public class KeyboardCommandSender : InputCommandSenderBase, IKeyboardCommandSender
    {
        public KeyboardCommandSender(ISettingsService settingsService, UdpClient udpClient)
            : base(settingsService, udpClient) { }

        public async Task SendKeyboardCommandAsync(KeyboardCommand command, string keyCode) =>
            await base.SendAsync(CommandType.Keyboard, command.ToString(), keyCode);

        public async Task SendTextEntryAsync(string textEntry) =>
            await base.SendAsync(CommandType.Keyboard, KeyboardCommand.TextEntry.ToString(), textEntry);
    }
}