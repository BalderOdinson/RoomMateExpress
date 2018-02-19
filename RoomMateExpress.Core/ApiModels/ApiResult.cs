using System;
using System.Collections.Generic;
using System.Text;

namespace RoomMateExpress.Core.ApiModels
{
    public struct ApiResult<TResult>
    {
        public ApiResult(string error, bool success, TResult result)
        {
            Error = error;
            Success = success;
            Result = result;
        }

        public string Error { get; }
        public bool Success { get; }
        public TResult Result { get; }
    }

    public struct ApiResult
    {
        public ApiResult(string error, bool success)
        {
            Error = error;
            Success = success;
        }

        public string Error { get; }
        public bool Success { get; }
    }
}
