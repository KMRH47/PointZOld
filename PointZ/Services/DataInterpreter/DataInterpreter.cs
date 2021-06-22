using System.Text;
using System.Threading.Tasks;

namespace PointZ.Services.DataInterpreter
{
    public class DataInterpreter : IDataInterpreter
    {
        public async Task InterpretAsync(byte[] bytes)
        {
            string data = Encoding.UTF8.GetString(bytes);
           
        }
    }
}