using System.Text;
using Xunit;

namespace PointZTest.Services.DataInterpreter
{
    public class DataInterpreterTests
    {
        public DataInterpreterTests()
        {
            
        }
        
        [Theory]
        [ClassData(typeof(DataInterpreterTestData))]
        public void CanInterpretAsync(byte[] bytes)
        {
            string message = Encoding.UTF8.GetString(bytes);

            switch (message)
            {
                case "Key":
                    
                default:
                    break;
            }

            var lol = bytes;
        }
    }
}