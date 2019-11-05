using NNNDataModel.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NNNDataModel.Logger
{
    public class BaseLog : BaseModel
    {
        public string TraceId { get; set; }
        public string Username { get; set; }

        public string LocalIPAddress { get; set; }
        public string RemoteIPAddress { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            string localIp = LocalIPAddress.IsNullOrEmpty() ? "NO_LOCAL_IP" : LocalIPAddress;
            string remoteIp = RemoteIPAddress.IsNullOrEmpty() ? "NO_REMOTE_IP" : RemoteIPAddress;
            string logString = $"[{CreatedTime.ToString("dd/MM/yyyy:HH:mm:ss UTC")}] [{TraceId} - {ID} - {Username.ToLengthyString(12)}] [{localIp.ToLengthyString(15)} | {remoteIp.ToLengthyString(15)}] [{Source}]";
            if (!Message.IsNullOrEmpty())
                logString += $"\n\t ==> { Message }";
            return logString;
        }
    }
}
