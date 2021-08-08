using System;
using System.IO;
using System.Threading.Tasks;

namespace PointZerver.Services.Logger
{
    public class FileLogger : ILogger
    {
        private readonly string path = $"{Environment.SpecialFolder.ApplicationData}/Logs";
        private readonly ConsoleLogger consoleLogger;

        /// <summary>
        /// Logs messages to .txt files.
        /// </summary>
        /// <param name="consoleLogger">Used to output exceptions to the console.</param>
        public FileLogger(ConsoleLogger consoleLogger)
        {
            try
            {
                this.consoleLogger = consoleLogger;
                if (Directory.Exists(this.path)) return;
                Directory.CreateDirectory(this.path);
            }
            catch (Exception e)
            {
                consoleLogger.Log($"Initialization of object {nameof(FileLogger)} failed: {e.Message}", this);
                throw;
            }
        }

        public async Task Log(string message, object contextSource)
        {
            try
            {
                await File.AppendAllTextAsync($"{this.path}/{contextSource}_log.txt", $"[{DateTime.Now}] {message}\n");
            }
            catch (Exception e)
            {
                await Task.Run(() => this.consoleLogger.Log($"{contextSource}: {e.Message}", this));
                throw;
            }
        } 
    }
}