using System.Threading.Tasks;

namespace PointZerver.Services.Logger
{
    public interface ILogger
    {
        Task Log(string message, object contextSource);
    }
}