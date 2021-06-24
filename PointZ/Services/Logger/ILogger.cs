using System.Threading.Tasks;

namespace PointZ.Services.Logger
{
    public interface ILogger
    {
        Task Log(string message);
    }
}