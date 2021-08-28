using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PointZ.Models.Command;

namespace PointZ.Services.CommandSender
{
    public class CommandSenderService : ICommandSenderService
    {
        private readonly UdpClient udpClient;
        private IPAddress ipAddress;

        public CommandSenderService(UdpClient udpClient) => this.udpClient = udpClient;

        /// <summary>
        /// Assigns the destination IP address. 
        /// </summary>
        /// <param name="ipAddress"></param>
        public void Bind(IPAddress ipAddress) => this.ipAddress = ipAddress;

        public Task SendAsync(MouseCommand command) =>
            InternalSendAsync(CommandType.Mouse, command.ToString(), null, this.ipAddress);
        public Task SendAsync(MouseCommand command, IPAddress ipAddress) =>
            InternalSendAsync(CommandType.Mouse, command.ToString(), null, ipAddress);
        public async Task SendAsync(MouseCommand command, string data) =>
            await InternalSendAsync(CommandType.Mouse, command.ToString(), data, this.ipAddress);
        public async Task SendAsync(MouseCommand command, string data, IPAddress ipAddress) =>
            await InternalSendAsync(CommandType.Mouse, command.ToString(), data, ipAddress);

        public async Task SendAsync(KeyboardCommand command, string data) =>
            await InternalSendAsync(CommandType.Keyboard, command.ToString(), data, this.ipAddress);
        public async Task SendAsync(KeyboardCommand command, string data, IPAddress ipAddress) =>
            await InternalSendAsync(CommandType.Keyboard, command.ToString(), data, ipAddress);

        private async Task InternalSendAsync(CommandType commandType, string command, string data, IPAddress ipAddress)
        {
            char commandT = (char) commandType;
            byte[] message = data == null
                ? Encoding.UTF8.GetBytes($"{commandT},{command}")
                : Encoding.UTF8.GetBytes($"{commandT},{command},{data}");

            IPEndPoint endPoint = new(ipAddress, 45454);
            await this.udpClient.SendAsync(message, message.Length, endPoint);
        }
    }
}