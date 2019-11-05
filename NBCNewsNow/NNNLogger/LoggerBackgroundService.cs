using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NNNDataModel.Logger;
using NNNLogger.Helpers;
using NNNDataModel.Helpers;

namespace NNNLogger
{
    class LoggerBackgroundService : IHostedService, IDisposable
    {
        private readonly IMemoryCache _cache;
        private readonly ILogWorker _worker;

        private Timer timer;

        public LoggerBackgroundService(IMemoryCache cache, ILogWorker worker)
        {
            _cache = cache;
            _worker = worker;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var traceLogs = _cache.Get<List<UserTraceLog>>(LoggerConstants.TraceLogCacheKey) ?? new List<UserTraceLog>();
            _cache.Remove(LoggerConstants.TraceLogCacheKey);

            var infoLogs = _cache.Get<List<UserLog>>(LoggerConstants.InfoLogCacheKey) ?? new List<UserLog>();
            _cache.Remove(LoggerConstants.InfoLogCacheKey);

            Task.Run(async () => {
                List<Task> insertTasks = new List<Task>();
                if (!traceLogs.IsNullOrEmpty())
                {
                    insertTasks.Add(_worker.InsertTraceLogs(traceLogs));
                }
                if (!infoLogs.IsNullOrEmpty())
                {
                    insertTasks.Add(_worker.InsertInfoLogs(infoLogs));
                }

                if (!insertTasks.IsNullOrEmpty())
                    await Task.WhenAll(insertTasks);
            }).GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
