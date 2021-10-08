using System.Net;
using System.Threading.Tasks;
using PointZ.Models.Command;

namespace PointZ.Services.CommandSender.Base
{
    public interface ICommandSender
    {
        void Bind(IPEndPoint ipEndPoint);
        Task SendAsync(MouseCommand command);
        Task SendAsync(MouseCommand command, string data);
        Task SendAsync(MouseCommand command, IPEndPoint ipEndPoint);
        Task SendAsync(MouseCommand command, string data, IPEndPoint ipEndPoint);
        Task SendAsync(KeyboardCommand command, string data);
        Task SendAsync(KeyboardCommand command, string data, IPEndPoint ipEndPoint);
    }
}