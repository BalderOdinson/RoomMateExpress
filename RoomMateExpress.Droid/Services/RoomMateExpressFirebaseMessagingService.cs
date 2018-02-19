using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Plugins.Messenger;
using RoomMateExpress.Core.Helpers.MvxMessengerHelpers;
using RoomMateExpress.Droid.Views;

namespace RoomMateExpress.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class RoomMateExpressFirebaseMessagingService : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            var notification = message.GetNotification();
            var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            switch (notification.ClickAction)
            {
                case "com.roommateexpress.android.MESSAGE":
                    var messageService = Mvx.Resolve<IMvxMessenger>();
                    messageService.Publish(new ChatMessage(this, Guid.Parse(message.Data["ChatId"]),
                        Guid.Parse(message.Data["MessageId"])));
                    if (activity is MessagesView messagesView)
                    {
                        if (messagesView.ViewModel.Chat.Id == Guid.Parse(message.Data["ChatId"]))
                            return;
                    }
                    break;
                case "com.roommateexpress.android.RESET":

                    var intent = new Intent(this, typeof(ResetPasswordView));
                    foreach (string key in message.Data.Keys)
                    {
                        intent.PutExtra(key, message.Data[key]);
                    }
                    activity.StartActivity(intent);
                    return;
                case "com.roommateexpress.android.PROFILE":
                    break;
            }

            SendNotification(message.GetNotification(), message.Data);
        }

        private void SendNotification(RemoteMessage.Notification notification, IDictionary<string, string> data)
        {
            var intent = new Intent(notification.ClickAction);
            intent.AddFlags(ActivityFlags.ClearTop);
            foreach (string key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new Notification.Builder(this)
                .SetSmallIcon((int)typeof(Resource.Drawable)
                    .GetField(notification.Icon).GetValue(null))
                .SetContentTitle(notification.Title)
                .SetContentText(notification.Body)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(0, notificationBuilder.Build());
        }
    }
}