using System.Net;
using System.Threading.Tasks;

namespace PointZ.Services.SessionEventHandler
{
    public interface ISessionEventHandlerService<in TEventArgs>
    {
        void Bind(IPAddress ipAddress);
        Task HandleAsync(TEventArgs e);
    }
}