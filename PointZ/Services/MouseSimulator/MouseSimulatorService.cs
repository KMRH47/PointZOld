using System.Threading.Tasks;
using InputSimulatorStandard;

namespace PointZ.Services.MouseSimulator
{
    public class MouseSimulatorService :IMouseSimulatorService
    {
        private readonly IMouseSimulator mouseSimulator;

        public  MouseSimulatorService(IMouseSimulator mouseSimulator)
        {
            this.mouseSimulator = mouseSimulator;
        }

        public Task Execute(object data)
        {
            this.mouseSimulator.LeftButtonClick();
            return null;
        }
    }
}