using System.Threading.Tasks;

namespace PointZClient.Services.Logger
{
    public interface ILogger
    {
        Task Log(string message, object contextSource);
    }
}