// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.App;
using Firebase.Messaging;

namespace Softeq.XToolkit.PushNotifications.Droid
{
    [Service(Exported = true)]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class XFirebaseMessagingService : FirebaseMessagingService
    {
        public static Action<string>? OnTokenRefreshed;
        public static Action<RemoteMessage>? OnNotificationReceived;

#pragma warning disable RECS0133 // Parameter name differs in base declaration
        public override void OnNewToken(string token)
#pragma warning restore RECS0133 // Parameter name differs in base declaration
        {
            OnTokenRefreshed?.Invoke(token);
        }

#pragma warning disable RECS0133 // Parameter name differs in base declaration
        public override void OnMessageReceived(RemoteMessage message)
#pragma warning restore RECS0133 // Parameter name differs in base declaration
        {
            OnNotificationReceived?.Invoke(message);
        }
    }
}
