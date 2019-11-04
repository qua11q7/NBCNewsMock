using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NNNDataModel.Helpers;
using System.Net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using NNNLogger;
using NNNDataModel;

namespace NBCNewsNow.Filters
{
    public class ResponseFilter : ActionFilterAttribute
    {
        private readonly ITraceLogger _traceLogger;
        private readonly INNNLogger _logger;

        public ResponseFilter(ITraceLogger traceLogger, INNNLogger<ResponseFilter> logger)
        {
            _traceLogger = traceLogger;
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items["result"] = new ResponseModel(_traceLogger.UserTraceId);

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null && !context.ExceptionHandled)
            {
                var result = context.HttpContext.Items["result"] as BaseResponseModel;
                result.ErrorMessage = context.Exception.ToStr();

                var request = context.HttpContext.Request;
                string source = $"{request.Method} {request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
                _logger.Log(LogPriority.Error, $"An error occured while performing a request to {source}: \r\n{context.Exception.ToStr()}");

                context.Exception = null;
                context.ExceptionHandled = true;

                context.Result = new BadRequestObjectResult(result);
            }

            base.OnActionExecuted(context);
        }
    }
}
