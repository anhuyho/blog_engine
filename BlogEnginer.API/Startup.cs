using BlogEngine.API.MediatoR.CQRS.Commands;
using BlogEngine.API.MediatoR.CQRS.Queries;
using BlogEngine.Data.FileManager;
using BlogEnginer.API.Data;
using BlogEnginer.API.Data.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyEndpoint = BlogEngine.DataTransferObject.MyEndpoint;

//using Microsoft.OpenApi.Models;
namespace BlogEngine
{

    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _endpoint = new DataTransferObject.MyEndpoint(configuration);
        }
        private MyEndpoint _endpoint = null;
        public IConfiguration Configuration { get; }

        
        
        public void ConfigureServices(IServiceCollection services)
        {
            //AddSqlServer(services);

            services.AddDbContext<AppDbContext>(options => options.UseSqlite("DataSource=blog.db"));

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

            //Register AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //Register MediatR
            services.AddMediatR(typeof(Startup));

            RegisterDependencyInjection(services);
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

        private void RegisterDependencyInjection(IServiceCollection services)
        {
            services.AddTransient<IRepository, Repository>();
            services.AddTransient<IFileManager, FileManager>();
            services.AddTransient<IGetAllPostsQuery, GetAllPostsQuery>();
            services.AddTransient<IGetPostByIdQuery, GetPostByIdQuery>();
            services.AddTransient<IUpdateAPostCommand, UpdateAPostCommand>();
            services.AddTransient<IPostAPostCommand, PostAPostCommand>();
            services.AddTransient<IDeleteAPostCommand, DeleteAPostCommand>();
        }
    }

}
