using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlogEngine.IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder

                    .ConfigureAppConfiguration((builderContext, config) =>
                    {
                        if (builderContext.HostingEnvironment.IsDevelopment())
                        {
                            config.AddJsonFile("appsettings.Development.json", optional: false);
                        }
                        else
                        {
                            config.AddJsonFile("appsettings.json", optional: false);
                        }
                    })
                    .UseStartup<Startup>();
                });
    }
}
