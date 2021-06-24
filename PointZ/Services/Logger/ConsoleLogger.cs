using System;
using System.Threading.Tasks;

namespace PointZ.Services.Logger
{
    public class ConsoleLogger : ILogger
    {
        public Task Log(string message) 
        {
            Console.WriteLine($"{DateTime.Now} {message}");
            return Task.CompletedTask;
        }
    }
}