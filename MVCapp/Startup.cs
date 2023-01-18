﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using MVCapp.Controllers;
using MVCapp.Interfaces;
using ProMVC.Repositories;
using System;
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

        public void ConfigureServices()
        {
            var builder = WebApplication.CreateBuilder();

            string connection = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<IUser, UserRepository>();
            builder.Services.AddTransient<UserController>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=User}/{action=Index}/{id?}");

            app.Run();
        }

        public async void Configure()
        {
        }
    }
}
