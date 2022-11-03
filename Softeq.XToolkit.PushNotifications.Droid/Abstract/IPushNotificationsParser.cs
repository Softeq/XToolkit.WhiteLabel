// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.OS;
using Firebase.Messaging;

namespace Softeq.XToolkit.PushNotifications.Droid.Abstract
{
    public interface IPushNotificationsParser
    {
        /// <summary>
        ///     Parse received notification object to <see cref="PushNotificationModel"/>.
        /// </summary>
        /// <param name="message">Received notification message.</param>
        /// <returns>Parsed notification.</returns>
        PushNotificationModel Parse(RemoteMessage message);

        /// <summary>
        ///     Parse received notification object to <see cref="PushNotificationModel"/>.
        /// </summary>
        /// <param name="bundle">Received notification extras.</param>
        /// <returns>Parsed notification.</returns>
        PushNotificationModel Parse(Bundle bundle);
    }
}
