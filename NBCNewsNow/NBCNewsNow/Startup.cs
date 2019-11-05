using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NBCNewsNow.DataModel;
using NBCNewsNow.Filters;
using NBCNewsNow.Middlewares;
using NBCNewsNow.Services;
using NNNDataContext.Context;
using NNNDataContext.Repositories;
using NNNLogger;
using NNNLogger.Helpers;
using Swashbuckle.AspNetCore.Swagger;

namespace NBCNewsNow
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var settingsSection = Configuration.GetSection("Settings");
            services.Configure<Settings>(settingsSection);

            // Database
            services.AddDbContext<NewsContext>(optionsBuilder => optionsBuilder.UseSqlServer(Configuration.GetConnectionString(DbConstants.ConnectionStringName)));
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<ITraceRepository, TraceRepository>();

            // Log
            services.ConfigureNNNLogger();

            // Services
            services.AddSingleton<ILogWorker, LogWorkerService>();
            services.AddScoped<INewsObtainerService, NewsObtainerService>();

            // Filters
            services.AddScoped<ResponseFilter>();

            // Utility
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            // Basic Authentication
            services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            // Swagger 
            services.AddSwaggerGen(c =>
            {
                c.DescribeAllEnumsAsStrings();
                c.SwaggerDoc("v1", new Info { Title = "NBC News Now Mock API", Version = "V1" });
                c.AddSecurityDefinition("Basic", new ApiKeyScheme()
                {
                    In = "header",
                    Name = "Authorization",
                    Description = "Basic Authentication",
                    Type = "basic"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Basic", new string[] { } }
                });
            });
        }

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

            app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            // Swagger configuration
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
            });

            app.ConfigureTraceLogger();
            app.UseAuthentication();
            app.ConfigureExceptionMiddleware();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
