using NNNDataModel.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NNNDataModel.Logger
{
    public class UserTraceLog : BaseLog
    {
        public bool HasPaylod { get; set; }
        public string PayloadBlob { get; set; }

        public void SetPayload(string payload) 
        {
            if (payload.IsNullOrEmpty())
                return;

            HasPaylod = true;
            PayloadBlob = payload;
        }

        public override string ToString()
        {
            string localIp = LocalIPAddress.IsNullOrEmpty() ? "NO_LOCAL_IP" : LocalIPAddress;
            string remoteIp = RemoteIPAddress.IsNullOrEmpty() ? "NO_REMOTE_IP" : RemoteIPAddress;
            string logString = $"[{CreatedTime.ToString("dd/MM/yyyy:HH:mm:ss UTC")}] [{TraceId} - {ID} - {Username.ToLengthyString(12)}] [{localIp.ToLengthyString(15)} | {remoteIp.ToLengthyString(15)}] [PL{(HasPaylod ? 1 : 0)}] [{Source}]";
            if (!Message.IsNullOrEmpty())
                logString += $"\n\t ==> { Message }";
            return logString;
        }
    }
}
