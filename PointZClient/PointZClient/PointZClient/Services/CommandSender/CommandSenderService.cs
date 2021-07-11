using System.Net.Sockets;

namespace PointZClient.Services.CommandSender
{
    public class CommandSenderService : ICommandSenderService
    {
        private readonly UdpClient udpClient;

        public CommandSenderService(UdpClient udpClient)
        {
            this.udpClient = udpClient;
        }

        public void Send(string command)
        {
            
        }
    }
}