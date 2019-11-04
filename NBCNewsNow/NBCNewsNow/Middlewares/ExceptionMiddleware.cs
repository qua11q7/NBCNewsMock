using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NNNDataModel;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NBCNewsNow.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public ExceptionMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    ILogger logger = scope.ServiceProvider.GetRequiredService<ILogger<ExceptionMiddleware>>();
                    logger.LogError($"An unhandled error occured while processing request!\r\nMessage: {ex.Message}\r\nSource: {ex.Source}\r\nTrace: {ex.StackTrace}\r\nSite: {ex.TargetSite}");
                }

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            ResponseModel rm = new ResponseModel
            {
                ErrorMessage = exception.Message,
                Result = exception
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(rm));
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder ConfigureExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
