using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RoomMateExpressWebApi.Helpers
{
    public class RoomMateExpressServiceResult
    {
        public RoomMateExpressServiceResult(Exception exception)
        {
            Exception = exception;
        }

        public string Error => Exception?.Message;

        public bool Success => Error == null;

        public Exception Exception { get; }
    }
}
