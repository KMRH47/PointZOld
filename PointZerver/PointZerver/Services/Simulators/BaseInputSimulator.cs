using System.Threading.Tasks;
using PointZerver.Services.Logger;

namespace PointZerver.Services.Simulators
{
    public abstract class BaseInputSimulator : IInputSimulator
    {
        protected readonly ILogger Logger;

        protected BaseInputSimulator(ILogger logger) => this.Logger = logger;
        
        public virtual string CommandId => null;
        public virtual Task ExecuteCommand(string data) => Task.CompletedTask;
    }
}