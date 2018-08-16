using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MotionDetectorWebApi.Repositories;
using MotionDetectorWebApi.Services;
using MotionFileWatcher;

namespace MotionDetectorWebApi
{
    public class Startup
    {
        private ILogger _logger;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", false);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            var filePath = Configuration["FilePath"];
            var fileWatcher = new FileWatcher(filePath);
            fileWatcher.Start();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IWebPushService, WebPushService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IDriveService, DriveService>();

            services.AddScoped<IWebPushRepository, WebPushRepository>();


            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .AllowAnyHeader());
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<MotionDetectorContext>(options => options.UseSqlite("Data Source=motion.db"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddFile(Configuration["Logging:PathFormat"]);

            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            app.UseMvc();

            _logger = loggerFactory.CreateLogger(typeof(Startup));
            _logger.LogInformation($"Starting MotionDetectorWebApi... Environment={env.EnvironmentName}");
        }
    }
}