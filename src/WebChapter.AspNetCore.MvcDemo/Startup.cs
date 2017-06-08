using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using WebChapter.AspNetCore.MvcDemo.Data;
using WebChapter.AspNetCore.MvcDemo.Data.DAL;
using WebChapter.AspNetCore.MvcDemo.Models;
using WebChapter.AspNetCore.MvcDemo.Options;
using WebChapter.AspNetCore.MvcDemo.Security;
using WebChapter.AspNetCore.MvcDemo.Services;

namespace WebChapter.AspNetCore.MvcDemo
{
    public class Startup
    {
        private enum OperatingSystem
        {
            OSX,
            Linux,
            Windows
        }

        private OperatingSystem CurrentOs
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return OperatingSystem.Linux;
                }
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return OperatingSystem.OSX;
                }
                return OperatingSystem.Windows;
            }
        }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{CurrentOs}.json", optional: false)
                .AddJsonFile($"appsettings.{CurrentOs}.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                //builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("DefaultConnection");
                var migrationsAssembly = Configuration.GetValue<string>("MigrationsAssembly");

                switch (CurrentOs)
                {
                    case OperatingSystem.Linux:
                    case OperatingSystem.OSX:
                        options.UseSqlite(
                            connectionString,
                            opts => opts.MigrationsAssembly(migrationsAssembly)
                        );
                        break;
                    case OperatingSystem.Windows:
                        options.UseSqlServer(
                            connectionString,
                            opts => opts.MigrationsAssembly(migrationsAssembly)
                        );
                        break;
                }
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            services.AddAuthorization(opts =>
            {
                // We create policies here which are then used with the AuthorizeAttribute.
                opts.AddPolicy(AuthorizationPolicies.Over21, policy => policy.AddRequirements(new AgeRequirement(21)));
                opts.AddPolicy(AuthorizationPolicies.Over18, policy => policy.AddRequirements(new AgeRequirement(18)));
                opts.AddPolicy(AuthorizationPolicies.EmployeeOnly, policy => policy.AddRequirements(new EmployeeRequirement()));
            });

            services.AddRouting(opts => opts.LowercaseUrls = true);

            // Add application services.
            services.AddTransient<IInventory, Inventory>();
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.Configure<InventoryOptions>(Configuration.GetSection(nameof(InventoryOptions)));

            // We want our AgeRequirementHandler as transient, since we want to refresh the injected
            // user manager for fresh data between requests.
            services.AddTransient<IAuthorizationHandler, AgeRequirementHandler>();
            // We will make EmployeeRequirementHandler a singleton since this checks the claims
            // of the currently logged in user without database access.
            services.AddSingleton<IAuthorizationHandler, EmployeeRequirementHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Sets up our ~/wwwroot directory as the root for all static files.
            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
