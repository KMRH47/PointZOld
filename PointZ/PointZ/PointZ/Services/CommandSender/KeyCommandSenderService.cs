using System.Net.Sockets;

namespace PointZ.Services.CommandSender
{
    public class KeyCommandSenderService : Base.CommandSender, IKeyCommandSenderService
    {
        public KeyCommandSenderService(UdpClient udpClient) : base(udpClient) { }
    }
}