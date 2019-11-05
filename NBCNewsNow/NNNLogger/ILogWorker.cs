using NNNDataModel.Logger;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NNNLogger
{
    public interface ILogWorker
    {
        Task InsertInfoLogs(IEnumerable<UserLog> logs);
        Task InsertTraceLogs(IEnumerable<UserTraceLog> traceLogs);
    }
}
