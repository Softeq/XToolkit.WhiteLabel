// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.PushNotifications
{
    public interface IPushNotificationsHandler
    {
        /// <summary>
        /// Handle the situation when push notification was received
        /// (happens in foreground for iOS; in foreground for Android 'notification' messages; in all cases for Android 'data' messages)
        /// </summary>
        /// <param name="pushNotification">Push notification model</param>
        void HandlePushNotificationReceived(PushNotificationModel pushNotification);

        /// <summary>
        /// Handle the situation when the application was open after user tap on push notification
        /// </summary>
        /// <param name="pushNotification">Push notification model</param>
        void HandlePushNotificationTapped(PushNotificationModel pushNotification);

        /// <summary>
        /// Handle the situation when silent push notification was received.
        /// In iOS notification is considered silent when 'content-available' property = 1.
        /// In Android notification is considered silent if incoming RemoteMessage does not contain body neither in 'notification', nor in 'data'
        /// </summary>
        /// <param name="pushNotification">Push notification model</param>
        void HandleSilentPushNotification(PushNotificationModel pushNotification);
    }
}
