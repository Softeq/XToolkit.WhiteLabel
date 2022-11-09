// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.OS;
using Firebase.Messaging;

namespace Softeq.XToolkit.PushNotifications.Droid.Abstract
{
    public interface IDroidPushNotificationsParser
    {
        /// <summary>
        ///     Parse received notification object to <see cref="PushNotificationModel"/>.
        /// </summary>
        /// <param name="message">Received notification message.</param>
        /// <param name="parsedPushNotificationModel">Parsed notification.</param>
        /// <returns><see langword="true"/> if notification has been parsed, <see langword="false"/> otherwise.</returns>
        bool TryParse(RemoteMessage message, out PushNotificationModel parsedPushNotificationModel);

        /// <summary>
        ///     Parse received notification object to <see cref="PushNotificationModel"/>.
        /// </summary>
        /// <param name="bundle">Received notification extras.</param>
        /// <param name="parsedPushNotificationModel">Parsed notification.</param>
        /// <returns><see langword="true"/> if notification has been parsed, <see langword="false"/> otherwise.</returns>
        bool TryParse(Bundle? bundle, out PushNotificationModel parsedPushNotificationModel);
    }
}
