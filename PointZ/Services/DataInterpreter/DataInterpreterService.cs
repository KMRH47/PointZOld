using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointZ.Services.Logger;
using PointZ.Services.MouseSimulator;

namespace PointZ.Services.DataInterpreter
{
    public class DataInterpreterService : IDataInterpreterService
    {
        private readonly IMouseSimulatorService mouseSimulatorService;
        private readonly ILogger logger;

        public DataInterpreterService(IMouseSimulatorService mouseSimulatorService, ILogger logger)
        {
            this.mouseSimulatorService = mouseSimulatorService;
            this.logger = logger;
        }

        public async Task InterpretAsync(byte[] bytes)
        {
            bytes = bytes.Where(b => b != 0).ToArray();
            string data = Encoding.UTF8.GetString(bytes);
            await this.logger.Log($"Received '{data}' ", this);

            if (data.Length == 0)
                PostCharacter(data[0]);

            switch (data)
            {
                case "#C":
                  //  MoveCursor(data);
                    break;
            }
        }

        private void PostCharacter(char c)
        {
        }

        private void MoveCursor(int x, int y)
        {
            this.mouseSimulatorService.MoveMouseBy(x, y);
        }

        private void Deserialize(string data)
        {
            (double, double) tuple = (0, 0);

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == 'x')
                {
                }
            }
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