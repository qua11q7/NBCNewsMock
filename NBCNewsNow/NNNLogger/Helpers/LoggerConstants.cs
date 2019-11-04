using NNNDataModel.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NNNLogger.Helpers
{
    public static class LoggerConstants
    {
        internal const string TraceLogCacheKey = "TraceLoggerCacheKey";
        internal const string InfoLogCacheKey = "InfoLoggerCacheKey";

        internal const LogPriority MinimumPriority = LogPriority.Info;

        public const string UsernamePlaceHolder = "[username]";
    }
}
