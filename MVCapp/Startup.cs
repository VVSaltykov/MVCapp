﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using MVCapp.Controllers;
using MVCapp.Interfaces;
using MVCapp.Models;
using MVCapp.Repositories;
using ProMVC.Repositories;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
namespace MVCapp
{
    public class Startup
    {
        public IConfigurationRoot _confString;

        [Obsolete]
        public Startup(IHostingEnvironment hostingEnvironment)
        {
            _confString = new ConfigurationBuilder().
                SetBasePath(hostingEnvironment.ContentRootPath).
                AddJsonFile("appsettings.json").
                Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(_confString.GetConnectionString("DefaultConnection")));
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/User/Login");
                });

            // Add services to the container.
            services.AddControllersWithViews(options =>
            {
                options.MaxModelValidationErrors = 50;
                options.EnableEndpointRouting = false;
            });
            services.AddTransient<IUser, UserRepository>();
            services.AddTransient<FileRepository>();
            services.AddTransient<EventLogRepository>();
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStatusCodePages();
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=User}/{action=Login}/{id?}");
            });
        }
    }
}
