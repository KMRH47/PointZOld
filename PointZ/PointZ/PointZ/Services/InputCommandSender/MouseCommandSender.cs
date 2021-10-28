using System.Net.Sockets;
using System.Threading.Tasks;
using PointZ.Models.Input;
using PointZ.Services.Settings;

namespace PointZ.Services.InputCommandSender
{
    public class MouseCommandSender : InputCommandSenderBase, IMouseCommandSender
    {
        public MouseCommandSender(ISettingsService settingsService, UdpClient udpClient)
            : base(settingsService, udpClient) { }
        
        public async Task SendMouseCommandAsync(MouseCommand command) =>
            await base.SendAsync(InputType.Mouse, command.ToString(), null);

        public async Task MoveMouseByAsync(int x, int y) => await InternalSendAsync(MouseCommand.MoveMouseBy, $"{x},{y}");
        public async Task MoveMouseToAsync(int x, int y) => await InternalSendAsync(MouseCommand.MoveMouseTo, $"{x},{y}");

        public async Task MoveMouseToPositionOnVirtualDesktopAsync(int x, int y) =>
            await InternalSendAsync(MouseCommand.MoveMouseToPositionOnVirtualDesktop, $"{x},{y}");

        public async Task HorizontalScrollAsync(int amount) =>
            await InternalSendAsync(MouseCommand.HorizontalScroll, amount.ToString());

        public async Task VerticalScrollAsync(int amount) =>
            await InternalSendAsync(MouseCommand.VerticalScroll, amount.ToString());

        private async Task InternalSendAsync(MouseCommand command, string data) =>
            await base.SendAsync(InputType.Mouse, command.ToString(), data);
    }
}