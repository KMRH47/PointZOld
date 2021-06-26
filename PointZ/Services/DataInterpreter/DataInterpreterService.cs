using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PointZ.Services.Logger;

namespace PointZ.Services.DataInterpreter
{
    public class DataInterpreterService : IDataInterpreterService
    {
        private readonly ILogger logger;
        private readonly IDictionary<string, string> commandMap;

        public DataInterpreterService(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task InterpretAsync(byte[] bytes)
        {
            string data = Encoding.UTF8.GetString(bytes);

            await this.logger.Log($"Received '{data}' ", this);
        }
    }
}