using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace NNNLogger.Helpers
{
    public static class LoggerExtensions
    {
        internal static CommonLogData GetCommonLogData(this IHttpContextAccessor httpContextAccessor)
        {
            CommonLogData commonData = new CommonLogData()
            {
                TraceId = "",
                Username = "anonymous",
                LocalIP = "0.0.0.0",
                RemoteIP = "0.0.0.0"
            };

            try
            {
                commonData.TraceId = httpContextAccessor.HttpContext.Items["userTraceId"].ToString();
            }
            catch (Exception ex)
            {
                commonData.TraceId = Guid.NewGuid().ToString();
            }

            try
            {
                commonData.Username = httpContextAccessor.HttpContext.User.Identity.Name;
            }
            catch (Exception ex)
            {
                commonData.Username = "anonymous";
            }

            try
            {
                commonData.LocalIP = httpContextAccessor.HttpContext.Connection.LocalIpAddress.ToString();
            }
            catch (Exception ex)
            {
                commonData.LocalIP = "";
            }

            try
            {
                commonData.RemoteIP = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            catch (Exception ex)
            {
                commonData.LocalIP = "";
            }

            return commonData;
        }

        public static IApplicationBuilder ConfigureTraceLogger(this IApplicationBuilder app)
        {
            return app.UseMiddleware<TraceLoggerMiddleware>();
        }

        public static IServiceCollection ConfigureNNNLogger(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddHostedService<LoggerBackgroundService>();

            services.AddTraceLogger();
            services.AddLogging(configuration => configuration.AddDebugLogger());

            services.AddScoped(typeof(INNNLogger<>), typeof(NNNLogger<>));

            return services;
        }

        public static IServiceCollection AddTraceLogger(this IServiceCollection services)
        {
            services.AddScoped<ITraceLogger, TraceLogger>();
            return services;
        }

        public static ILoggingBuilder AddDebugLogger(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerProvider, InfoLoggerProvider>();
            return builder;
        }
    }
}
