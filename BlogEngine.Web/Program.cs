using System;
using System.IO;
using System.Linq;
using BlogEngine.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogEngine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            try
            {
                var scope = host.Services.CreateScope();

                var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userMgr = scope.ServiceProvider
                    .GetRequiredService<UserManager<IdentityUser>>();
                var roleMgr = scope.ServiceProvider
                    .GetRequiredService<RoleManager<IdentityRole>>();

                ctx.Database.EnsureCreated();

                var adminRole = new IdentityRole("Admin");
                if (!ctx.Roles.Any())
                {
                    roleMgr.CreateAsync(adminRole).GetAwaiter().GetResult();
                }

                if (!ctx.Users.Any(u => u.UserName == "admin"))
                {
                    var adminUser = new IdentityUser
                    {
                        UserName = "admin",
                        Email = "admin@test.com"
                    };
                    userMgr.CreateAsync(adminUser, "password").GetAwaiter().GetResult();
                    userMgr.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception here" + ex.Message);
                Console.WriteLine("End");
            }



            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {

            var hos = new WebHostBuilder()
                        .UseKestrel()
                        .ConfigureAppConfiguration((builderContext, config) =>
                        {
                            config.AddJsonFile("appsettings.json", optional: false);
                        })
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseIISIntegration()
                        .UseStartup<Startup>();
            //.Build();
            return hos;

        }


        public static IWebHostBuilder CreateWebHostBuilde(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }
            

    }
}
