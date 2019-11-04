using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace NNNLogger
{
    public class InfoLoggerProvider : ILoggerProvider
    {
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InfoLoggerProvider(IMemoryCache cache, IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new InfoLogger(categoryName, _cache, _httpContextAccessor);
        }

        public void Dispose() { }
    }
}
