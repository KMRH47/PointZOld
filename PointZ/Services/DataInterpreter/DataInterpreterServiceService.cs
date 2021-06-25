using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PointZ.Services.DataInterpreter
{
    public class DataInterpreterServiceService : IDataInterpreterService
    {
        private readonly IDictionary<string, string> commandMap;
        
        public DataInterpreterServiceService()
        {
            // Service to handle some action...    
        }
        
        public async Task InterpretAsync(byte[] bytes)
        {
            string data = Encoding.UTF8.GetString(bytes);
           
        }
    }
}