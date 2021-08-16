using System.Diagnostics;
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

        public CommandSenderService(UdpClient udpClient) => this.udpClient = udpClient;

        public async Task SendAsync(MouseCommand command, string data, string address) =>
            await InternalSendAsync("M", command.ToString(), data, address);

        public async Task SendAsync(KeyboardCommand command, string data, string address) =>
            await InternalSendAsync("K", command.ToString(), data, address);

        private async Task InternalSendAsync(string commandType, string command, string data, string address)
        {
            Debug.WriteLine($"Command: {command}\nData: {data}\nAddress: {address}");
            byte[] message = Encoding.UTF8.GetBytes($"{commandType},{command},{data}");
          
            IPAddress ipAddress = IPAddress.Parse(address);
            IPEndPoint endPoint = new(ipAddress, 45454);
            await this.udpClient.SendAsync(message, message.Length, endPoint);
        }
    }
}