using System;
using System.Threading.Tasks;
using PointZerver.Services.Logger;

namespace PointZerver.Services.Simulators
{
    public abstract class BaseInputSimulator : IInputSimulator
    {
        protected readonly ILogger Logger;

        protected BaseInputSimulator(ILogger logger) => this.Logger = logger;
        
        public virtual string CommandId => null;

        public virtual async Task ExecuteCommand(string[] data)
        {
            string command = data[1];
            
            if (command == null)
                throw new NullReferenceException(
                    $"No command sent with the data passed to {nameof(ExecuteCommand)} in {nameof(BaseInputSimulator)}.");
            
            await this.Logger.Log($"Executing command {command}.",this);
        }
    }
}