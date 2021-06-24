using System;
using System.IO;
using System.Threading.Tasks;

namespace PointZ.Services.Logger
{
    public class FileLogger : ILogger
    {
        private readonly string filePath;

        public FileLogger(string contextName)
        {
            string path = $"{Environment.SpecialFolder.ApplicationData}/Logs/";
            this.filePath = $"{path}/{contextName}.txt";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            File.Create($"{this.filePath}").Close();
        }

        public async Task Log(string message)
        {
            await File.AppendAllTextAsync(this.filePath, message);
        }
    }
}