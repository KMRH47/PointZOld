using System.Threading.Tasks;
using PointZClient.Models.Command;

namespace PointZClient.Services.CommandSender
{
    public interface ICommandSenderService
    {
        Task SendAsync(MouseCommand command, string data, string address);
        Task SendAsync(KeyboardCommand command, string data, string address);
    }
}