using System;
using BlogEngine.Data.FileManager;
using BlogEngine.DataTransferObject;
using BlogEnginer.API.Data;
using BlogEnginer.API.Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

//using Microsoft.OpenApi.Models;
namespace BlogEngine
{
    public partial class Startup
    {
        private void AddSqlServer(IServiceCollection services)
        {
            var connectionString = Configuration["DefaultConnection"];
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            services.AddEntityFrameworkSqlite()
                .AddDbContext<AppDbContext>((serviceProvider, options) =>
                    options.UseSqlite("Data Source=blog.db")
                        .UseInternalServiceProvider(serviceProvider));
        }

        private void AddIdentity(IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>(
                    option =>
                    {
                        option.Password.RequireDigit = false;
                        option.Password.RequireNonAlphanumeric = false;
                        option.Password.RequireUppercase = false;
                        option.Password.RequiredLength = 6;
                    }
                )
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>();
        }

        private void AddCors(IServiceCollection services)
        {
            services.AddCors(config =>
                config.AddPolicy("AllowAll",
                    p => p.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()));
        }

        private void AddAuthentication(IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", config =>
                {
                    config.Authority = Contanst.IdentityServerEndPoint + "/";

                    config.Audience = "Blog.API";
                });

            
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ToDo API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Shayne Boyer",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
            });
        }
    }

}
