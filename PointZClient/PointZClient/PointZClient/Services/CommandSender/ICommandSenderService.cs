using System.Threading.Tasks;
using PointZClient.Models.CursorBehavior;

namespace PointZClient.Services.CommandSender
{
    public interface ICommandSenderService
    {
        Task Send(string command);
        Task Send(CursorBehavior cursorBehavior);
    }
}