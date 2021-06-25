using System;
using System.Threading.Tasks;

namespace PointZ.Services.Logger
{
    public class ConsoleLogger : ILogger
    {
        public Task Log(string message, object contextSource)
        {
            Console.WriteLine($"{DateTime.Now} {contextSource}: {message}");
            return Task.CompletedTask;
        }
    }
}
