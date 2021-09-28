using System.Threading.Tasks;

namespace PointZ.Services.SessionMessageHandler
{
    public interface ISessionMessageHandlerService
    {
        Task SendMessageAsync(string message);
    }
}