// Developed for PAWS-HALO by Softeq Development Corporation
// http://www.softeq.com

using Firebase.Messaging;

namespace Softeq.XToolkit.PushNotifications.Abstract
{
    /// <summary>
    /// Basic interface for the event handler from the <see cref="FirebaseMessagingService"/>
    /// </summary>
    internal interface IDroidFirebaseMessagingHandler
    {
        /// <summary>
        ///    Callback called when push notification is received.
        /// </summary>
        /// <param name="message">Received remote message.</param>
        void OnNotificationReceived(RemoteMessage message);

        /// <summary>
        ///     Callback called when push notification token has changed.
        /// </summary>
        /// <param name="token">New push notification token.</param>
        void OnPushTokenRefreshed(string token);
    }
}
