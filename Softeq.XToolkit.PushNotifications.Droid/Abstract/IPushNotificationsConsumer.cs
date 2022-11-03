// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Firebase.Messaging;

namespace Softeq.XToolkit.PushNotifications.Droid.Abstract
{
    /// <summary>
    ///     Basic interface for push notification consumer on Android platform.
    /// </summary>
    public interface IPushNotificationsConsumer
    {
        /// <summary>
        ///    Callback called when push notification is received.
        /// </summary>
        /// <param name="message">Received remote message.</param>
        /// <returns><see langword="true"/> if consumer has handled remote message, <see langword="false"/> otherwise.</returns>
        bool TryHandleNotification(RemoteMessage message);

        /// <summary>
        ///    Callback called when application is launched by push notification.
        /// </summary>
        /// <param name="intent">Launch intent.</param>
        /// <returns><see langword="true"/> if consumer has handled remote message, <see langword="false"/> otherwise.</returns>
        bool TryHandlePushNotificationIntent(Intent intent);

        /// <summary>
        ///     Callback called when push notification token has changed.
        /// </summary>
        /// <param name="token">New push notification token.</param>
        void OnPushTokenRefreshed(string token);

        /// <summary>
        ///     Callback called when application has unregistered from push notifications.
        /// </summary>
        /// <returns>
        ///     A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task OnUnregisterFromPushNotifications();
    }
}
