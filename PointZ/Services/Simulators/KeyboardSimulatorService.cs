using System.Threading.Tasks;
using InputSimulatorStandard;

namespace PointZ.Services.Simulators
{
    public class KeyboardSimulatorService : IInputSimulatorService
    {
        private readonly IKeyboardSimulator keyboardSimulator;

        public KeyboardSimulatorService(IKeyboardSimulator keyboardSimulator)
        {
            this.keyboardSimulator = keyboardSimulator;
        }

        public string CommandId => "#K";

        public Task Execute(object o)
        {
            return null;
        }
    }
}