using System.Threading.Tasks;

namespace PointZerver.Services.SimulatorInterpreter
{
    public interface ISimulatorInterpreterService
    {
        Task InterpretAsync(byte[] bytes);
    }
}