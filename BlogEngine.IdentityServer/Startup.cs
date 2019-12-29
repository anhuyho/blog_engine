using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using BlogEngine.IdentityServer.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlogEngine.IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("DataSource=identityServer.db"));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders(); ;

            //services.AddIdentity<IdentityUser, IdentityRole>(config =>
            //    {
            //        config.Password.RequiredLength = 4;
            //        config.Password.RequireDigit = false;
            //        config.Password.RequireNonAlphanumeric = false;
            //        config.Password.RequireUppercase = false;
            //    })
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();


            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "BlogEngine.IdentityServer.Cookie";
                config.LoginPath = "/Identity/Account/Login";
                config.LogoutPath = "/Identity/Account/Logout";
            });

            services.AddIdentityServer()
                .AddAspNetIdentity<IdentityUser>()

                
                .AddInMemoryApiResources(IdentityServer.Configuration.GetApis())
                .AddInMemoryIdentityResources(IdentityServer.Configuration.GetIdentityResources())
                .AddInMemoryClients(IdentityServer.Configuration.GetClients(Configuration))
                //.AddTestUsers(new List<TestUser> { 
                //    new TestUser
                //    {
                //        Claims =  new Claim[]{
                //        new Claim(JwtClaimTypes.Name, "Alice Smith"),
                //        new Claim(JwtClaimTypes.GivenName, "Alice"),
                //        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                //        new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                //        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                //        new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                //        new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                //    }
                //    }
                //})
                .AddDeveloperSigningCredential();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            app.Use(async (context, next) =>
            {
                context.Request.Scheme = "https";
                await next.Invoke();
            });

            app.UseIdentityServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
