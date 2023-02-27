// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.App;
using Firebase.Messaging;
using Softeq.XToolkit.PushNotifications.Abstract;
using Softeq.XToolkit.PushNotifications.Droid.Services;
using Softeq.XToolkit.WhiteLabel;

namespace Softeq.XToolkit.PushNotifications.Droid
{
    [Service(Exported = true)]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    internal class XFirebaseMessagingService : FirebaseMessagingService
    {
        private readonly IDroidFirebaseMessagingHandler _messagingHandler;

        public XFirebaseMessagingService()
        {
            _messagingHandler = Dependencies.Container.Resolve<DroidPushNotificationsService>();
        }

#pragma warning disable RECS0133 // Parameter name differs in base declaration
        public override void OnNewToken(string token)
#pragma warning restore RECS0133 // Parameter name differs in base declaration
        {
            _messagingHandler.OnPushTokenRefreshed(token);
        }

#pragma warning disable RECS0133 // Parameter name differs in base declaration
        public override void OnMessageReceived(RemoteMessage message)
#pragma warning restore RECS0133 // Parameter name differs in base declaration
        {
            _messagingHandler.OnNotificationReceived(message);
        }
    }
}
