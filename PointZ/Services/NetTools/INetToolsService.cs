using System.Threading;
using System.Threading.Tasks;

namespace PointZ.Services.NetTools
{
    public interface INetToolsService
    {
        Task<string> GetLocalIpv4Address(CancellationToken cancellationToken);
    }
}