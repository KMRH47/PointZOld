using System.IO;
using System.Net.Sockets;
using Microsoft.Extensions.DependencyInjection;
using PointZ.Services.Logger;

namespace PointZTest
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Add services
            services.AddTransient<StringWriter>();
            services.AddTransient<UdpClient>();
            services.AddTransient<ILogger, ConsoleLogger>();
        }
    }
}