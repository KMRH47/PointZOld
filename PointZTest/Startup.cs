using System.IO;
using System.Net.Sockets;
using Microsoft.Extensions.DependencyInjection;
using PointZ.Services.Logger;
using PointZTest.Services.LoggerService;

namespace PointZTest
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Add services
            services.AddTransient<StringWriter>();
            services.AddTransient<UdpClient>();
            services.AddTransient<ConsoleLoggerTests>();
            services.AddTransient<ILogger, ConsoleLogger>();
        }
    }
}