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
                completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound | UNNotificationPresentationOptions.Badge); // TODO: make customizable
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
            var actionIdentifier = response.ActionIdentifier;
            var userInfo = response.Notification.Request.Content.UserInfo;

            if (response.IsCustomAction)
            {
                var textInput = (response as UNTextInputNotificationResponse)?.UserText;
                _pushNotificationsReceiver.OnMessageCustomActionInvoked(userInfo, actionIdentifier, textInput);
            }
            else if (response.IsDefaultAction)
            {
                _pushNotificationsReceiver.OnMessageTapped(userInfo);
            }
            else if (response.IsDismissAction)
            {
                _pushNotificationsReceiver.OnMessageCustomActionInvoked(userInfo, actionIdentifier, null);
            }

            completionHandler.Invoke();
        }
    }
}
