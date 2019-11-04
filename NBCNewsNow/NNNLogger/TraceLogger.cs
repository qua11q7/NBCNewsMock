using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using NNNLogger.Helpers;
using System.Collections.Generic;
using NNNDataModel.Log;
using NNNDataModel.Helpers;

namespace NNNLogger
{
    public interface ITraceLogger
    {
        string UserTraceId { get; }

        void Log();
        void Log(string message);
        void Log(string message, string payload);
    }

    public class TraceLogger : ITraceLogger
    {
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly string _userTraceId;
        private readonly string _username;
        private readonly string _source;
        private readonly string _localIpAddress;
        private readonly string _remoteIpAddress;

        public string UserTraceId => _userTraceId;

        public TraceLogger(IMemoryCache cache, IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;

            var request = _httpContextAccessor.HttpContext.Request;
            _source = $"{request.Method} {request.Scheme}://{request.Host}{request.Path}{request.QueryString}";

            CommonLogData commonData = _httpContextAccessor.GetCommonLogData();
            _userTraceId = commonData.TraceId;
            _username = commonData.Username;
            _localIpAddress = commonData.LocalIP;
            _remoteIpAddress = commonData.RemoteIP;
        }

        public void Log()
        {
            Log("", null);
        }

        public void Log(string message)
        {
            Log(message, null);
        }

        public void Log(string message, string payload)
        {
            UserTraceLog newLog = new UserTraceLog()
            {
                Username = _username,
                TraceId = _userTraceId,
                Source = _source,
                LocalIPAddress = _localIpAddress,
                RemoteIPAddress = _remoteIpAddress,
                Message = message,
                HasPaylod = !payload.IsNullOrEmpty(),
                PayloadBlob = payload,
            };

            var logs = _cache.Get<List<UserTraceLog>>(LoggerConstants.TraceLogCacheKey) ?? new List<UserTraceLog>();
            logs.Add(newLog);
            _cache.Set(LoggerConstants.TraceLogCacheKey, logs);
        }
    }
}
