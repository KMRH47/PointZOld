using System.Net.Sockets;
using System.Threading.Tasks;
using PointZ.Models.Command;

namespace PointZ.Services.CommandSender
{
    public class TouchCommandSenderService : Base.CommandSender, ITouchCommandSenderService
    {
        public TouchCommandSenderService(UdpClient udpClient) : base(udpClient) { }

        public async Task MoveMouseByAsync(int x, int y) => await base.SendAsync(MouseCommand.MoveMouseBy, $"{x},{y}");
        public async Task MoveMouseToAsync(int x, int y) => await base.SendAsync(MouseCommand.MoveMouseTo, $"{x},{y}");

        public async Task MoveMouseToPositionOnVirtualDesktopAsync(int x, int y) =>
            await base.SendAsync(MouseCommand.MoveMouseToPositionOnVirtualDesktop, $"{x},{y}");

        public async Task HorizontalScrollAsync(int amount) =>
            await base.SendAsync(MouseCommand.HorizontalScroll, amount.ToString());

        public async Task VerticalScrollAsync(int amount) =>
            await base.SendAsync(MouseCommand.VerticalScroll, amount.ToString());
    }
}