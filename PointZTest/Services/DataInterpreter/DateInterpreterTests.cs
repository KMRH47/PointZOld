using System.Text;
using System.Threading.Tasks;

namespace PointZTest.Services.DataInterpreter
{
    public class DateInterpreterTests
    {
        public async Task InterpretAsync(byte[] bytes)
        {
            string data = Encoding.UTF8.GetString(bytes);

            switch (data)
            {
                case "key":
                    break;
                case "cursor":
                    break;
            }
        }
    }
}