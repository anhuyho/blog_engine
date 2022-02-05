using BlogEngine.DataTransferObject;
using BlogEngine.Web.FileManager;
using BlogEngine.Web.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlogEngine.Web
{
    public class Startup
    {
        //private IConfiguration _config;
        Endpoint _endpoint;
        public Startup(IConfiguration config)
        {
            //_config = config;
            _endpoint = new Endpoint(config);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config => {
                    config.DefaultScheme = "Cookie";
                    config.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookie")
                .AddOpenIdConnect("oidc", config => {

                    //https://huyblog-identityserver.azurewebsites.net/
                    config.Authority = _endpoint.Id4 + "/"; 
                    config.ClientId = "client_id_mvc";
                    config.ClientSecret = "client_secret_mvc";
                    config.SaveTokens = true;
                    config.ResponseType = "code";
                    config.SignedOutCallbackPath = "/Home/Index";
                    //config.CallbackPath = "/signin-oidc";


                    config.ClaimActions.DeleteClaim("amr");
                    config.ClaimActions.DeleteClaim("s_hash");
                    config.ClaimActions.MapUniqueJsonKey("RawCoding.Grandma", "rc.garndma");


                    config.GetClaimsFromUserInfoEndpoint = true;

                    // configure scope
                    config.Scope.Clear();
                    config.Scope.Add("openid");
                    //config.Scope.Add("rc.scope");
                    config.Scope.Add(Contanst.BlogAPI);
                    config.Scope.Add("offline_access");

                });

            services.AddHttpClient();
            services.AddControllersWithViews().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.PropertyNamingPolicy = null;
                o.JsonSerializerOptions.DictionaryKeyPolicy = null;
            }).AddRazorRuntimeCompilation();
            services.AddHttpContextAccessor();
            services.AddTransient<IControllerHelpers, ControllerHelpers>();
            services.AddTransient<IFileManager, FileManager.FileManager>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();
            loggerFactory.AddFile("Logs/ts-{Date}.txt");


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
