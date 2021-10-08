using System.Net;
using System.Threading.Tasks;

namespace PointZ.Services.SessionEventHandler
{
    public interface ISessionEventHandlerService<in TEventArgs>
    {
        void Bind(IPEndPoint ipEndPoint);
        Task HandleAsync(TEventArgs e);
    }
}