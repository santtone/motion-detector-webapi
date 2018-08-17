using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MotionDetectorWebApi.Repositories;
using MotionDetectorWebApi.Services;
using Swashbuckle.AspNetCore.Swagger;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace MotionDetectorWebApi
{
    /// Startup
    public class Startup
    {
        private ILogger _logger;

        /// Startup constructor
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", false);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        /// This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHostedService, FileWatcher>();

            services.AddScoped<IWebPushService, WebPushService>();
            services.AddScoped<IFileService, FileService>();
            services.AddTransient<IDriveService, DriveService>();

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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Motion Detector Web API", Version = "v1", Description = ".NET Core Web API 2.1"});
                c.IncludeXmlComments(GetXmlCommentsPath());
            });
        }

        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            loggerFactory.AddFile(Configuration["LogPathFormat"]);

            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.  
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MD Web API - v1");
            });

            app.UseMvc();

            _logger = loggerFactory.CreateLogger(typeof(Startup));
            _logger.LogInformation($"Starting MotionDetectorWebApi... Environment={env.EnvironmentName}");
        }

        private static string GetXmlCommentsPath()
        {
            return System.IO.Path.Combine(System.AppContext.BaseDirectory, "MotionDetectorWebApi.xml");
        }
    }
}