using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using ASJ.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ASJ.Models;
using Microsoft.Extensions.Logging;

namespace ASJ
{
    public class Startup
    {
        private IHostingEnvironment _env;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public class ApplicationVariables
        {
            public string ReferenceYear { get; set; }
            public string EmailHost { get; set; }
            public string EmailSender { get; set; }
            public string DeveloperEmails { get; set; }

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ASJDbContext>(options =>
                            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ASJLegacyContext>(options =>
                            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = "X-CSRF-TOKEN-COOKIEFDCRP";
                options.Cookie.HttpOnly = true;

                options.HeaderName = "X-CSRF-TOKEN-HEADERFDCRP";
            });


            services.AddIdentity<AppIdentityUser, AppIdentityRole>()
                    .AddEntityFrameworkStores<AppIdentityDbContext>()
                    .AddDefaultTokenProviders();

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = environment == EnvironmentName.Development;

            if (!isDevelopment)
            {
                 services.ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/Home/Index";
                    options.LogoutPath = "/Home/Logout";
                    options.AccessDeniedPath = "/Security/AccessDenied";
                    options.SlidingExpiration = true;
                    options.Cookie = new CookieBuilder
                    {
                        HttpOnly = true,
                        Name = ".ASJ.Security.Cookie",
                        Path = "/",
                        SameSite = SameSiteMode.Lax,

                        SecurePolicy = CookieSecurePolicy.Always
                    };
                });
            }
            else
            {
                services.ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/Home/Index";
                    options.LogoutPath = "/Home/Logout";
                    options.AccessDeniedPath = "/Security/AccessDenied";
                    options.SlidingExpiration = true;
                    options.Cookie = new CookieBuilder
                    {
                        HttpOnly = true,
                        Name = ".ASJ.Security.Cookie",
                        Path = "/",
                        SameSite = SameSiteMode.Lax,

                        SecurePolicy = CookieSecurePolicy.SameAsRequest
                    };
                });

            }

            services.AddAuthorization(options => {
                options.AddPolicy("AdminsOnly", policy =>
                  policy.RequireClaim(ClaimTypes.Role, "sysadmin"));
                options.AddPolicy("NonPOC", policy =>
                 policy.RequireRole("dataentry","impersonation","contactsview","contactsedit", "assignmentedit","eventmanagerview","eventmanageredit", "usermanager"));
            });
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddSession(options =>
                {
                    options.IdleTimeout = TimeSpan.FromMinutes(30);
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = _env.IsDevelopment()
                    ? CookieSecurePolicy.SameAsRequest
                    : CookieSecurePolicy.Always;
                });
            //services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddMvc();

            //this works, may need to uncomment to add it too
            //services.AddMvc(config =>
            //{
            //    // If it's Production, enable HTTPS
            //    if (!isDevelopment)      
            //    {
            //        config.Filters.Add(new RequireHttpsAttribute());
            //    }
            //});

            var appVariables = Configuration.GetSection("ApplicationVariables");
            services.Configure<ApplicationVariables>(appVariables);
  
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
