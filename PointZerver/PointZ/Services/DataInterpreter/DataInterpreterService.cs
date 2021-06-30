using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PointZ.Extensions;
using PointZ.Services.Logger;
using PointZ.Services.Simulators;

namespace PointZ.Services.DataInterpreter
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
                this.logger.Log($"Initialization of object {nameof(DataInterpreterService)} failed: {e.Message}", this);
                throw;
            }
        }

        public async Task InterpretAsync(byte[] bytes)
        {
            try
            {
                byte[] shavedBytes = await bytes.CopyRemovingNulls();
                string data = Encoding.UTF8.GetString(shavedBytes);
                string[] deserializedData = data.Split(',');

                this.inputSimulatorServiceMap.TryGetValue(deserializedData[0],
                    out IInputSimulator inputSimulatorService);
                if (inputSimulatorService == null) throw new NullReferenceException();
                await inputSimulatorService.ExecuteCommand(deserializedData);
            }
            catch (Exception e)
            {
                await this.logger.Log($"{e.Message}\n{e.StackTrace}", this);
                throw;
            }
        }
    }
}