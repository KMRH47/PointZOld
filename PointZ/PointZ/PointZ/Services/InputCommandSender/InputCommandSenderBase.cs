using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PointZ.Models.Input;
using PointZ.Services.Settings;

namespace PointZ.Services.InputCommandSender
{
    public abstract class InputCommandSenderBase
    {
        private readonly ISettingsService settingsService;
        private readonly UdpClient udpClient;

        protected InputCommandSenderBase(ISettingsService settingsService, UdpClient udpClient)
        {
            this.settingsService = settingsService;
            this.udpClient = udpClient;
        }

        protected async Task SendAsync(InputType inputType, string command, string data) =>
            await InternalSendAsync((char)inputType, command, data);

        private async Task InternalSendAsync(char commandType, string command, string data)
        {
            byte[] message = data == null
                ? Encoding.UTF8.GetBytes($"{commandType},{command}")
                : Encoding.UTF8.GetBytes($"{commandType},{command},{data}");
            
            await this.udpClient.SendAsync(message, message.Length, this.settingsService.ServerIpEndPoint);
        }
    }
}