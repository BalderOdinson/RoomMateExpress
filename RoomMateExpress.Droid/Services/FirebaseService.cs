using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Iid;
using Firebase.Messaging;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.Settings;

namespace RoomMateExpress.Droid.Services
{
    public class FirebaseService : IFirebaseService
    {
        public void InitializeToken()
        {
            ApplicationData.FirebaseToken = FirebaseInstanceId.Instance.Token;
        }

        public void Subsrcibe()
        {
            try
            {
                if (ApplicationData.CurrentUserViewModel != null)
                    FirebaseMessaging.Instance.SubscribeToTopic(ApplicationData.CurrentUserViewModel.Id.ToString());

            }
            catch (Exception e)
            {
                Log.Debug("FirebaseException", e.Message);
            }
        }

        public void Unsubscribe()
        {
            try
            {
                if (ApplicationData.CurrentUserViewModel != null)
                    FirebaseMessaging.Instance.UnsubscribeFromTopic(ApplicationData.CurrentUserViewModel.Id.ToString());

            }
            catch (Exception e)
            {
                Log.Debug("FirebaseException", e.Message);
            }
        }
    }
}