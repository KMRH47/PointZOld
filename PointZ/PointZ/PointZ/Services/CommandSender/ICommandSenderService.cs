using System.Net;
using System.Threading.Tasks;
using PointZ.Models.Command;

namespace PointZ.Services.CommandSender
{
    public interface ICommandSenderService
    {
        void Bind(IPAddress ipAddress);
        Task SendAsync(MouseCommand command);
        Task SendAsync(MouseCommand command, string data);
        Task SendAsync(MouseCommand command, IPAddress ipAddress);
        Task SendAsync(MouseCommand command, string data, IPAddress ipAddress);
        Task SendAsync(KeyboardCommand command, string data);
        Task SendAsync(KeyboardCommand command, string data, IPAddress ipAddress);
    }
}