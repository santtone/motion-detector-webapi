using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MotionDetectorWebApi.Config;
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

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IWebPushService, WebPushService>();
            services.AddScoped<IFileService, FileService>();
            services.AddTransient<IDriveService, DriveService>();

            services.AddScoped<IWebPushRepository, WebPushRepository>();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Audience = Configuration["Azure:AD:ClientId"];
                    options.Authority = Configuration["Azure:AD:Instance"] +
                                        Configuration["Azure:AD:TenantId"];
                });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .AllowAnyHeader());
            });

            services.AddMvc()
                .AddJsonOptions(
                    options => { options.SerializerSettings.ContractResolver = new MotionDetectorContractResolver(); }
                )
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<MotionDetectorContext>(options => options.UseSqlite("Data Source=motion.db"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "Motion Detector Web API",
                        Version = "v1",
                        Description = ".NET Core Web API 2.1"
                    });
                //c.IncludeXmlComments(GetXmlCommentsPath());
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

            //app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            UseProxy(app);

            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.  
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "MD Web API - v1"); });

            app.UseMvc();

            _logger = loggerFactory.CreateLogger(typeof(Startup));
            _logger.LogInformation($"Starting MotionDetectorWebApi... Environment={env.EnvironmentName}");
            _logger.LogInformation($"App Base Path={env.ContentRootPath}");
            _logger.LogInformation($"wwwroot Path={env.WebRootPath}");
        }

        private static string GetXmlCommentsPath()
        {
            return System.IO.Path.Combine(System.AppContext.BaseDirectory, "MotionDetectorWebApi.xml");
        }

        private void UseProxy(IApplicationBuilder app)
        {
            //Proxy /stream to the motion server
            app.Map("/stream", builder =>
            {
                builder.Run(async context =>
                {
                    if (!context.User.Identity.IsAuthenticated)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Authentication required for path /stream");
                    }
                });
            });
            app.MapWhen(
                context => context.Request.Path.Value.StartsWith(@"/stream", StringComparison.OrdinalIgnoreCase),
                builder => builder.RunProxy(new ProxyOptions
                {
                    Scheme = Configuration["StreamProxy:Scheme"],
                    Host = Configuration["StreamProxy:Host"],
                    Port = Configuration["StreamProxy:Port"]
                }));
        }
    }
}