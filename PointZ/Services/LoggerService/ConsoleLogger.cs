using System;

namespace PointZ.Services.LoggerService
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine(message);
    }
}