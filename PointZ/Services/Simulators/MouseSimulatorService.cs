using System.Threading.Tasks;
using InputSimulatorStandard;

namespace PointZ.Services.Simulators
{
    public class MouseSimulatorService : IInputSimulatorService
    {
        private readonly IMouseSimulator mouseSimulator;

        public MouseSimulatorService(IMouseSimulator mouseSimulator)
        {
            this.mouseSimulator = mouseSimulator;
        }

        public string CommandId => "#C";

        public Task Execute(object data)
        {
            this.mouseSimulator.MoveMouseBy(x, y);
            return null;
        }
    }
}