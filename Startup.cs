using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using zims.Helpers;
using ZIMS.Data.Services.Users;
using ZIMS.Helpers;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ZIMS.Data;
using AutoMapper;
using ZIMS.Data.Entities;
using ZIMS.Models.Users;

namespace ZIMS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //this code is added in case I want to deploy to Heroku later:
            string DATABASE_URL = Environment.GetEnvironmentVariable("DATABASE_URL_STR");
            string connectionString = (DATABASE_URL == null ? Configuration.GetConnectionString("DefaultConnection") : DATABASE_URL);
            Console.WriteLine($"Using connection string: {connectionString}");
            //using AppDbContext with Postgres database
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString)
            );
            services.AddCors();
            services.AddControllersWithViews();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IUserService, UserService>();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
           {
               configuration.RootPath = "client-app/build";
           });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader());
            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
           {
               spa.Options.SourcePath = "client-app";

               if (env.IsDevelopment())
               {
                   spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                   spa.UseReactDevelopmentServer(npmScript: "start");
               }
           });
        }
    }
}
