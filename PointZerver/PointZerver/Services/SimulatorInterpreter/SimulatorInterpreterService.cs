using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PointZerver.Services.Logger;
using PointZerver.Services.Simulators;

namespace PointZerver.Services.SimulatorInterpreter
{
    public class SimulatorInterpreterService : ISimulatorInterpreterService
    {
        private readonly IDictionary<string, IInputSimulatorService> inputSimulatorServiceMap =
            new Dictionary<string, IInputSimulatorService>();
        private readonly ILogger logger;

        public SimulatorInterpreterService(ILogger logger, IEnumerable<IInputSimulatorService> inputSimulatorServices)
        {
            try
            {
                this.logger = logger;

                foreach (IInputSimulatorService inputSimulatorService in inputSimulatorServices)
                    this.inputSimulatorServiceMap.Add(inputSimulatorService.CommandId, inputSimulatorService);
            }
            catch (Exception e)
            {
                logger.Log($"Initialization of object {nameof(SimulatorInterpreterService)} failed: {e.Message}", this);
            }
        }

        public async Task InterpretAsync(byte[] bytes)
        {
            try
            {
                string data = Encoding.UTF8.GetString(bytes);
                this.inputSimulatorServiceMap.TryGetValue(data[0].ToString(), out IInputSimulatorService inputSimulatorService);
                if (inputSimulatorService == null) throw new NullReferenceException();
                await inputSimulatorService.ExecuteCommand(data);
            }
            catch (Exception e)
            {
                await this.logger.Log($"{e.Message}\n{e.StackTrace}", this);
            }
        }
    }
}