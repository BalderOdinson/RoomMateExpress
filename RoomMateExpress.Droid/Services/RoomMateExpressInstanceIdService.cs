using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Iid;
using Microsoft.EntityFrameworkCore;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Settings;

namespace RoomMateExpress.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class RoomMateExpressInstanceIdService : FirebaseInstanceIdService
    {
        public override void OnTokenRefresh()
        {
            ApplicationData.FirebaseToken = FirebaseInstanceId.Instance.Token;
        }
    }
}