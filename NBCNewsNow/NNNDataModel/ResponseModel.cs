using System;
using System.Collections.Generic;
using System.Text;

namespace NNNDataModel
{
    public class BaseResponseModel
    {
        public bool IsSuccessfull => string.IsNullOrEmpty(ErrorMessage);
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public string TraceId { get; set; }

        public BaseResponseModel() { }

        public BaseResponseModel(string traceId)
        {
            TraceId = traceId;
        }
    }

    public class ResponseModel : BaseResponseModel
    {
        public bool HasResult => Result != null;
        public object Result { get; set; }

        public ResponseModel() { }

        public ResponseModel(string traceId) : base(traceId) { }
    }

    public class ResponseModel<T> : BaseResponseModel
    {
        public bool HasResult => Result != null;
        public T Result { get; set; }

        public ResponseModel() { }

        public ResponseModel(string traceId) : base(traceId) { }
    }
}
