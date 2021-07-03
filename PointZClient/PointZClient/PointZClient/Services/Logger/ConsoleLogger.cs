using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PointZClient.Services.Logger
{
    public class ConsoleLogger : ILogger
    {
        public Task Log(string message, object contextSource)
        {
            Debug.WriteLine($"{DateTime.Now} {contextSource}: {message}");
            return Task.CompletedTask;
        }
    }
}