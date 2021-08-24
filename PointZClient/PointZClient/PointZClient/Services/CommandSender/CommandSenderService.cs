using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PointZClient.Models.Command;

namespace PointZClient.Services.CommandSender
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
            InternalSendAsync("M", command.ToString(), null, this.ipAddress);
        public Task SendAsync(MouseCommand command, IPAddress ipAddress) =>
            InternalSendAsync("M", command.ToString(), null, ipAddress);
        public async Task SendAsync(MouseCommand command, string data) =>
            await InternalSendAsync("M", command.ToString(), data, this.ipAddress);
        public async Task SendAsync(MouseCommand command, string data, IPAddress ipAddress) =>
            await InternalSendAsync("M", command.ToString(), data, ipAddress);

        public async Task SendAsync(KeyboardCommand command, string data) =>
            await InternalSendAsync("K", command.ToString(), data, this.ipAddress);
        public async Task SendAsync(KeyboardCommand command, string data, IPAddress ipAddress) =>
            await InternalSendAsync("K", command.ToString(), data, ipAddress);

        private async Task InternalSendAsync(string commandType, string command, string data, IPAddress ipAddress)
        {
            byte[] message = data == null
                ? Encoding.UTF8.GetBytes($"{commandType},{command}")
                : Encoding.UTF8.GetBytes($"{commandType},{command},{data}");

            IPEndPoint endPoint = new(ipAddress, 45454);
            await this.udpClient.SendAsync(message, message.Length, endPoint);
        }
    }
}