using System;
using System.Collections.Generic;
using System.Linq;
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
            byte[] buffer = RemoveEmptyEntries(bytes);
            string data = Encoding.UTF8.GetString(buffer);
            await this.logger.Log($"Received '{data}' ", this);
        }

        private static byte[] RemoveEmptyEntries(IEnumerable<byte> bytes) => bytes.Where(b => b != 0).ToArray();
    }
}