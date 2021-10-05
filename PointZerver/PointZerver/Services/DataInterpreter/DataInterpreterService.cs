using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using PointZerver.Extensions;
using PointZerver.Services.Logger;
using PointZerver.Services.Simulators;

namespace PointZerver.Services.DataInterpreter
{
    public class DataInterpreterService : IDataInterpreterService
    {
        private readonly IDictionary<string, IInputSimulator> inputSimulatorServiceMap =
            new Dictionary<string, IInputSimulator>();
        private readonly ILogger logger;

        public DataInterpreterService(ILogger logger, params IInputSimulator[] inputSimulatorServices)
        {
            try
            {
                this.logger = logger;

                foreach (IInputSimulator inputSimulatorService in inputSimulatorServices)
                    this.inputSimulatorServiceMap.Add(inputSimulatorService.CommandId, inputSimulatorService);
            }
            catch (Exception e)
            {
                logger.Log($"Initialization of object {nameof(DataInterpreterService)} failed: {e.Message}", this);
            }
        }

        public async Task InterpretAsync(byte[] bytes)
        {
            try
            {
                byte[] shavedBytes = await bytes.CopyRemovingNulls();
                string data = Encoding.UTF8.GetString(shavedBytes);

                this.inputSimulatorServiceMap.TryGetValue(data[0].ToString(), out IInputSimulator inputSimulatorService);
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