using System.Threading.Tasks;

namespace PointZ.Services.MouseSimulator
{
    public interface IMouseSimulatorService
    {
        Task MoveMouseBy(int x, int y);
    }
}