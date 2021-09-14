using System.Net;
using System.Threading.Tasks;
using PointZ.Services.TouchEvent;

namespace PointZ.Services.SessionTouchEventHandler
{
    public interface ISessionTouchEventHandlerService
    {
        void Bind(IPAddress ipAddress);
        Task HandleAsync(TouchEventArgs e);
    }
}