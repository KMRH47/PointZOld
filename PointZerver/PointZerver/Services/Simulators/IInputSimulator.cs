using System.Threading.Tasks;

namespace PointZerver.Services.Simulators
{
    public interface IInputSimulator
    {
        public string CommandId { get; }
        Task ExecuteCommand(string[] data);
    }
}