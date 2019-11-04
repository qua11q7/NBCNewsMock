using Microsoft.Extensions.DependencyInjection;
using NNNDataContext.Repositories;
using NNNDataModel.Log;
using NNNLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NBCNewsNow.Services
{
    public class LogWorkerService : ILogWorker
    {
        private readonly IServiceProvider _serviceProvider;

        public LogWorkerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task InsertInfoLogs(IEnumerable<UserLog> logs)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var logRepo = scope.ServiceProvider.GetRequiredService<ILogRepository>();
                await logRepo.CreateMany(logs);
            }
        }

        public async Task InsertTraceLogs(IEnumerable<UserTraceLog> traceLogs)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var traceRepo = scope.ServiceProvider.GetRequiredService<ITraceRepository>();
                await traceRepo.CreateMany(traceLogs);
            }
        }
    }
}
