using System;
using BlogEngine.API.Data.Repository;
using BlogEngine.Data.FileManager;
using BlogEngine.DataTransferObject;
using BlogEnginer.API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

//using Microsoft.OpenApi.Models;
namespace BlogEngine.API
{
    
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _endpoint = new Endpoint(configuration);
        }

        public IConfiguration Configuration { get; }
        private Endpoint _endpoint;
        
        
        public void ConfigureServices(IServiceCollection services)
        {
            //AddSqlServer(services);

            services.AddDbContext<AppDbContext>(options => options.UseSqlite("DataSource=blog.db"));

            //services.AddIdentity<IdentityUser, IdentityRole>(
            //        option =>
            //        {
            //            option.Password.RequireDigit = false;
            //            option.Password.RequireNonAlphanumeric = false;
            //            option.Password.RequireUppercase = false;
            //            option.Password.RequiredLength = 6;
            //        }
            //    )
            //    .AddRoles<IdentityRole>()
            //    .AddEntityFrameworkStores<IdentityDbContext>();


            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", config =>
                {
                    config.Authority = _endpoint.Id4 + "/";

                    config.Audience = "Blog.API";

                });

            services.AddCors(confg =>
                confg.AddPolicy("AllowAll",
                    p => p.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()));


            services.AddControllers().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.PropertyNamingPolicy = null;
                o.JsonSerializerOptions.DictionaryKeyPolicy = null;
            });
            //AddSwagger(services);
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


            services.AddTransient<IRepository, Repository>();
            services.AddTransient<IFileManager, FileManager>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog API");
                c.RoutePrefix = string.Empty;
            });
            
            app.UseStaticFiles();
            app.UseCors("AllowAll");
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

}
