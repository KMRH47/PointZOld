using System.Net;
using System.Threading.Tasks;
using PointZ.Services.TouchEvent;

namespace PointZ.SessionEventHandler
{
    public interface ISessionEventHandlerService
    {
        void Bind(IPAddress ipAddress);
        Task HandleAsync(TouchEventArgs e);
    }
}