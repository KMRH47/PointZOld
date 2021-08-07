using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PointZClient.Models.CursorBehavior;

namespace PointZClient.Services.CommandSender
{
    public class CommandSenderService : ICommandSenderService
    {
        private readonly UdpClient udpClient;

        public CommandSenderService(UdpClient udpClient)
        {
            this.udpClient = udpClient;
        }

        public async Task Send(string command)
        {
        }

        public async Task Send(CursorBehavior cursorBehavior)
        {
            byte[] message = Encoding.UTF8.GetBytes($"{cursorBehavior.PosX},{cursorBehavior.PosY}");
            Task<int> task = this.udpClient.SendAsync(message, message.Length);
        }
    }
}