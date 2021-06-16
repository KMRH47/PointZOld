using System;

namespace PointZ.Services.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine(message);
    }
}