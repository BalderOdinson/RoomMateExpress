using System;
using System.Collections.Generic;
using System.Text;

namespace FirebaseCloudMessagingApi.Exceptions
{
    public class FcmApiException : Exception
    {
        public FcmApiException(string msg) : base(msg)
        {
            
        }
    }
}
