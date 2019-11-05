using NNNDataModel.Logger;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;
using NNNDataModel.Helpers;
using Microsoft.Extensions.Logging;
using NNNLogger.Helpers;

namespace NNNLogger
{
    public interface INNNLogger {
        void Log(LogPriority priority, string message);
    }

    public interface INNNLogger<T> : INNNLogger
    {
    }

    public class NNNLogger<T> : INNNLogger<T>
    {
        private readonly ILogger _logger;

        public NNNLogger(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void Log(LogPriority priority, string message)
        {
            // This level can also be obtained from a setting file.
            if (priority >= LoggerConstants.MinimumPriority)
                _logger.Log((LogLevel)priority, message);
        }
    }
}
