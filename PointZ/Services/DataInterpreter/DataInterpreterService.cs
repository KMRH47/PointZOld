using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
            bytes = bytes.Where(b => b != 0).ToArray();
            string data = Encoding.UTF8.GetString(bytes);
            await this.logger.Log($"Received '{data}' ", this);

            if (data.Length == 0)
                PostCharacter(data[0]);

            if (ContainsNonAnsiCharacter(data)) 
                InterpretAnsiCharacter(data);
            else InterpretNonAnsiCharacter(data);
        }

        private static void PostCharacter(char c)
        {
        }

        private static void InterpretNonAnsiCharacter(string data)
        {
        }

        private static void InterpretAnsiCharacter(string data)
        {
        }

        private static bool ContainsNonAnsiCharacter(string data) => data.Any(c => c >= 255);
    }
}