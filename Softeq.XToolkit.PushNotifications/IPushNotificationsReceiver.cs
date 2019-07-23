// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.PushNotifications
{
    public interface IPushNotificationsReceiver
    {
        // Receiving messages

        /// <summary>
        /// Method which must be called when push notification message is received
        /// Is called internally
        /// </summary>
        /// <param name="pushNotification">Push notification object</param>
        /// <param name="inForeground">Flag indicating if push notification was received in foreground,
        /// can be false on Android for data push notifications</param>
        void OnMessageReceived(object pushNotification, bool inForeground);

        /// <summary>
        /// Callback which must be called when push notification message was tapped (the app was opened by user tap on push notification)
        /// On iOS is called internally
        /// On Android must be called on starting activity created with specific extras
        /// </summary>
        /// <param name="pushNotification">Push notification object</param>
        void OnMessageTapped(object pushNotification);
    }
}
