using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PointZ.Services.DataInterpreter
{
    public class DataInterpreterService : IDataInterpreterService
    {
        private readonly IDictionary<string, string> commandMap;
        
        public DataInterpreterService()
        {
            // Service to handle some action...    
        }
        
        public async Task InterpretAsync(byte[] bytes)
        {
            string data = Encoding.UTF8.GetString(bytes);
            
            
            
           
        }
    }
}