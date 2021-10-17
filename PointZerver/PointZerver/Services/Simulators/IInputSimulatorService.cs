using System.Threading.Tasks;

namespace PointZerver.Services.Simulators
{
    public interface IInputSimulatorService
    {
        public string CommandId { get; } 
        Task ExecuteCommand(string data);
    }
}