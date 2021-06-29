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
        private const byte ProtocolLength = 2;
        private readonly IDictionary<string, IInputSimulatorService> inputSimulatorServiceMap =
            new Dictionary<string, IInputSimulatorService>();
        private readonly ILogger logger;

        public DataInterpreterService(ILogger logger, params IInputSimulatorService[] inputSimulatorServices)
        {
            try
            {
                this.logger = logger;

                foreach (IInputSimulatorService inputSimulatorService in inputSimulatorServices)
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
                await bytes.CutFromFirstNullCharacter();
                var data = Encoding.UTF8.GetString(bytes);
                string[] deserializedData = data.Split(',');
                string command = deserializedData[0];
                string value = deserializedData[1];

                this.inputSimulatorServiceMap.TryGetValue(command, out IInputSimulatorService inputSimulatorService);
                if (inputSimulatorService == null) throw new NullReferenceException();
                await inputSimulatorService.Execute(value);
            }
            catch (NullReferenceException e)
            {
                await this.logger.Log($"{e.Message}\n{e.StackTrace}", this);
                throw;
            }
            catch (Exception e)
            {
                await this.logger.Log($"{e.Message}\n{e.StackTrace}", this);
                throw;
            }
        }
    }
}