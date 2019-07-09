// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.PushNotifications
{
    public interface IPushNotificationsCallbacks
    {
        // Receiving messages

        /// <summary>
        /// Callback which must be called when push notification message is received
        /// </summary>
        /// <param name="pushNotification">Push notification object</param>
        void OnMessageReceived(object pushNotification);

        /// <summary>
        /// Callback which must be called when push notification message was tapped (the app was opened by user tap on push notification)
        /// </summary>
        /// <param name="pushNotification">Push notification object</param>
        void OnMessageTapped(object pushNotification);
    }
}
