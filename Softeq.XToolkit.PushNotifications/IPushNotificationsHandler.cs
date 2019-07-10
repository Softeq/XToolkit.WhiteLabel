// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.PushNotifications
{
    public interface IPushNotificationsHandler
    {
        /// <summary>
        /// Handle registration result (for instance, you can unregister if needed, try registration again or just log message)
        /// If one of the parameters is false - we won't be able to receive push notifications
        /// </summary>
        /// <param name="isRegisteredInSystem">Value that indicates if we are registered to push notifications in system with token stored in IPushTokenStorageService</param>
        /// <param name="isSavedOnServer">Value that indicates if current token was saved on server</param>
        void OnPushRegistrationCompleted(bool isRegisteredInSystem, bool isSavedOnServer);

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
