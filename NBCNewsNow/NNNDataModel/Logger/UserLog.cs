using NNNDataModel.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NNNDataModel.Logger
{
    public class UserLog : BaseLog
    {
        public LogPriority Priority { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }

        public override string ToString()
        {
            string localIp = LocalIPAddress.IsNullOrEmpty() ? "NO_LOCAL_IP" : LocalIPAddress;
            string remoteIp = RemoteIPAddress.IsNullOrEmpty() ? "NO_REMOTE_IP" : RemoteIPAddress;
            string logString = $"[{CreatedTime.ToString("dd/MM/yyyy:HH:mm:ss UTC")}] [{TraceId} - {ID} - {Username.ToLengthyString(12)}] [{Priority.ToString().ToLengthyString(8)} - {EventId} - {EventName}] [{localIp.ToLengthyString(15)} | {remoteIp.ToLengthyString(15)}] [{Source}]";
            if (!Message.IsNullOrEmpty())
                logString += $"\n\t ==> { Message }";
            return logString;
        }
    }
}
