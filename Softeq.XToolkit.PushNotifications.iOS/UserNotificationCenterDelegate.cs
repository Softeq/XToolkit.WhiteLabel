// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using UserNotifications;

namespace Softeq.XToolkit.PushNotifications.iOS
{
    public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        private readonly IPushNotificationsReceiver _pushNotificationsReceiver;
        private readonly bool _showForegroundNotificationsInSystem;

        public UserNotificationCenterDelegate(
            IPushNotificationsReceiver pushNotificationsReceiver,
            bool showForegroundNotificationsInSystem)
        {
            _pushNotificationsReceiver = pushNotificationsReceiver;
            _showForegroundNotificationsInSystem = showForegroundNotificationsInSystem;
        }

        // Handle Foreground Notifications
        public override void WillPresentNotification(
            UNUserNotificationCenter center,
            UNNotification notification,
            Action<UNNotificationPresentationOptions> completionHandler)
        {
            _pushNotificationsReceiver.OnMessageReceived(notification.Request.Content.UserInfo, true);
            if (_showForegroundNotificationsInSystem)
            {
                completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound);
            }
            else
            {
                completionHandler(UNNotificationPresentationOptions.None);
            }
        }

        // Called when notification is tapped
        public override void DidReceiveNotificationResponse(
            UNUserNotificationCenter center,
            UNNotificationResponse response,
            Action completionHandler)
        {
            _pushNotificationsReceiver.OnMessageTapped(response.Notification.Request.Content.UserInfo);
            completionHandler.Invoke();
        }
    }
}
