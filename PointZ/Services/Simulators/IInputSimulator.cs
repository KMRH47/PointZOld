using System.Threading.Tasks;

namespace PointZ.Services.Simulators
{
    public interface IInputSimulator
    {
        public string CommandId { get; }
        Task ExecuteCommand(string[] data);
    }
}