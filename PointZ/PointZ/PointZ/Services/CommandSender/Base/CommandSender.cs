using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PointZ.Models.Command;

namespace PointZ.Services.CommandSender.Base
{
    public class CommandSender : ICommandSender
    {
        private readonly UdpClient udpClient;
        private IPEndPoint ipEndPoint;

        protected CommandSender(UdpClient udpClient) => this.udpClient = udpClient;

        /// <summary>
        /// Assigns the destination IP address. 
        /// </summary>
        /// <param name="ipEndPoint"></param>
        public void Bind(IPEndPoint ipEndPoint) => this.ipEndPoint = ipEndPoint;

        public Task SendAsync(MouseCommand command) =>
            InternalSendAsync(CommandType.Mouse, command.ToString(), null, this.ipEndPoint);
        public Task SendAsync(MouseCommand command, IPEndPoint ipEndPoint) =>
            InternalSendAsync(CommandType.Mouse, command.ToString(), null, ipEndPoint);
        public async Task SendAsync(MouseCommand command, string data) =>
            await InternalSendAsync(CommandType.Mouse, command.ToString(), data, this.ipEndPoint);
        public async Task SendAsync(MouseCommand command, string data, IPEndPoint ipEndPoint) =>
            await InternalSendAsync(CommandType.Mouse, command.ToString(), data, ipEndPoint);

        public async Task SendAsync(KeyboardCommand command, string data) =>
            await InternalSendAsync(CommandType.Keyboard, command.ToString(), data, this.ipEndPoint);
        public async Task SendAsync(KeyboardCommand command, string data, IPEndPoint ipEndPoint) =>
            await InternalSendAsync(CommandType.Keyboard, command.ToString(), data, ipEndPoint);

        private async Task InternalSendAsync(CommandType commandType, string command, string data, IPEndPoint ipEndPoint)
        {
            char commandT = (char) commandType;
            byte[] message = data == null
                ? Encoding.UTF8.GetBytes($"{commandT},{command}")
                : Encoding.UTF8.GetBytes($"{commandT},{command},{data}");

            await this.udpClient.SendAsync(message, message.Length, ipEndPoint);
        }
    }
}