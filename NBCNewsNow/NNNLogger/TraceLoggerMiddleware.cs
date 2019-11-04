using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NNNLogger
{
    public class TraceLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _services;

        public TraceLoggerMiddleware(RequestDelegate next, IServiceProvider services)
        {
            _next = next;
            _services = services;
        }

        public async Task Invoke(HttpContext context)
        {
            string message;
            string userTraceId = context.Request?.Headers["userTraceId"];
            if (string.IsNullOrEmpty(userTraceId))
            {
                userTraceId = Guid.NewGuid().ToString();
                message = $"Received a request and created a new TraceId";
            }
            else
            {
                message = $"Received a request with TraceId";
            }

            context.Items["userTraceId"] = userTraceId;

            using (var scope = _services.CreateScope())
            {
                ITraceLogger logger = scope.ServiceProvider.GetRequiredService<ITraceLogger>();
                logger.Log(message);
            }

            await _next(context);
        }
    }
}
