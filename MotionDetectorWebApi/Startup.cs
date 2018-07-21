using GoogleDriveClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MotionDetectorWebApi.Repositories;
using MotionDetectorWebApi.Services;
using MotionFileWatcher;

namespace MotionDetectorWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var filePath = Configuration["FilePath"];
            var fileWatcher = new FileWatcher(filePath);
            fileWatcher.Start();

            var drive = new GDrive(
                Configuration["GoogleDrive:AppServiceEmail"],
                Configuration["GoogleDrive:KeyFilePath"],
                Configuration["GoogleDrive:KeyPassword"]);
            drive.ListFiles();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IWebPushService, WebPushService>();
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            app.UseMvc();
        }
    }
}