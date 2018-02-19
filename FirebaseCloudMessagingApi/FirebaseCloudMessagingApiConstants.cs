using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FirebaseCloudMessagingApi
{
    public static class FirebaseCloudMessagingApiConstants
    {
        public const string MaxTtl = "2419200s";
        public const string MinTtl = "0s";
        public const string DefaultSound = "default";
        public const string FcmScope = @"https://www.googleapis.com/auth/firebase.messaging";
        public const string FcmSeverUrl = @"https://fcm.googleapis.com";
        public const string TokenError = "Couldn't retrieve access token. Check your credentials and try again.";
    }
}
