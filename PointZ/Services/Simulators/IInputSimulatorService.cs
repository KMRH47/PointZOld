using System.Threading.Tasks;

namespace PointZ.Services.Simulators
{
    public interface IInputSimulatorService
    {
        public string CommandId { get; }
        Task Execute(object o);
    }
}