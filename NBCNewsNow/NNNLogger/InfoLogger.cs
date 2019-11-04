using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NNNDataModel.Helpers;
using NNNDataModel.Log;
using NNNLogger.Helpers;

namespace NNNLogger
{
    public interface IInfoLogger : ILogger { 
    }

    public class InfoLogger : IInfoLogger
    {
        private readonly string _categoryName;
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly string _userTraceId;
        private readonly string _username;
        private readonly string _localIpAddress;
        private readonly string _remoteIpAddress;

        public InfoLogger(string categoryName, IMemoryCache cache, IHttpContextAccessor httpContextAccessor)
        {
            _categoryName = categoryName;
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;

            CommonLogData commonData = _httpContextAccessor.GetCommonLogData();
            _userTraceId = commonData.TraceId;
            _username = commonData.Username;
            _localIpAddress = commonData.LocalIP;
            _remoteIpAddress = commonData.RemoteIP;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel > (LogLevel)LoggerConstants.MinimumPriority;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            UserLog debugLog = new UserLog()
            {
                Username = _username,
                TraceId = _userTraceId,
                LocalIPAddress = _localIpAddress,
                RemoteIPAddress = _remoteIpAddress,
                Source = _categoryName,
                EventId = eventId.Id,
                EventName = eventId.Name,
                Priority = (LogPriority)logLevel,
                Message = formatter(state, exception)
            };

            var logs = _cache.Get<List<UserLog>>(LoggerConstants.InfoLogCacheKey) ?? new List<UserLog>();
            logs.Add(debugLog);
            _cache.Set(LoggerConstants.InfoLogCacheKey, logs);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new NoopDisposable();
        }

        private class NoopDisposable : IDisposable
        {
            public void Dispose() { }
        }
    }
}
