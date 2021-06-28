using System.Threading.Tasks;

namespace PointZ.Services.MouseSimulator
{
    public interface IMouseSimulatorService
    {
        Task Execute(object data);
    }
}