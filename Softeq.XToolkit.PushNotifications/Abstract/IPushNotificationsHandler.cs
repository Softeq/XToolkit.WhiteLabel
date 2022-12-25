// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.PushNotifications.Abstract
{
    public interface IPushNotificationsHandler
    {
        /// <summary>
        ///     Handle registration result (for instance, you can unregister if needed, try registration again or just log message).
        ///     <para/>
        ///     If one of the parameters is <see langword="false"/> - we won't be able to receive push notifications.
        /// </summary>
        /// <param name="isRegisteredInSystem">
        ///     Value that indicates if we are registered to push notifications in system with token stored in
        /// </param>
        /// <param name="isSavedOnServer">Value that indicates if current token was saved on server.</param>
        void OnPushRegistrationCompleted(bool isRegisteredInSystem, bool isSavedOnServer);

        /// <summary>
        ///     Handle the result of push notifications permission request on iOS (this is not relevant for Android).
        ///     <para/>
        ///     Note that push notifications registration will be initiated anyway.
        ///     You might want to use this callback for logging or analytics.
        /// </summary>
        /// <param name="permissionsGranted">
        ///     <see langword="true"/> if user allowed notification permissions,
        ///     <see langword="false"/> otherwise.
        /// </param>
        void OnPushPermissionsRequestCompleted(bool permissionsGranted);

        /// <summary>
        ///     Handle the situation when push notification was received.
        ///     <para/>
        ///     This happens:
        ///     <para/>
        ///     1. In foreground for iOS.
        ///     <para/>
        ///     2. In foreground for Android 'notification' messages.
        ///     <para/>
        ///     3. In all cases for Android 'data' messages.
        /// </summary>
        /// <param name="pushNotification">Push notification model.</param>
        /// <param name="inForeground">
        ///     Flag indicating if push notification was received in foreground.
        ///     <para/>
        ///     Can be <see langword="false"/> on Android for data push notifications
        ///     - should be considered when handling notification.
        /// </param>
        void HandlePushNotificationReceived(PushNotificationModel pushNotification, bool inForeground);

        /// <summary>
        ///     Handle the situation when the application was open after user tap on push notification.
        /// </summary>
        /// <param name="pushNotification">Push notification model.</param>
        void HandlePushNotificationTapped(PushNotificationModel pushNotification);

        /// <summary>
        ///     Handle the situation when silent push notification was received.
        ///     <para/>
        ///     In iOS notification is considered silent when 'content-available' property = 1.
        ///     <para/>
        ///     In Android notification is considered silent if incoming RemoteMessage does not contain body neither in 'notification',
        ///     nor in 'data'.
        /// </summary>
        /// <param name="pushNotification">Push notification model.</param>
        void HandleSilentPushNotification(PushNotificationModel pushNotification);
    }
}
