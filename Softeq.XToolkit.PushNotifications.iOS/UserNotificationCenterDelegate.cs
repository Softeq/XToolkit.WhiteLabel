// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using UserNotifications;

namespace Softeq.XToolkit.PushNotifications.iOS
{
    public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        private readonly IPushNotificationsReceiver _pushNotificationsReceiver;
        private readonly ForegroundNotificationOptions _showForegroundNotificationsInSystemOptions;

        public UserNotificationCenterDelegate(
            IPushNotificationsReceiver pushNotificationsReceiver,
            ForegroundNotificationOptions showForegroundNotificationsInSystemOptions)
        {
            _pushNotificationsReceiver = pushNotificationsReceiver;
            _showForegroundNotificationsInSystemOptions = showForegroundNotificationsInSystemOptions;
        }

        // Handle Foreground Notifications
        public override void WillPresentNotification(
            UNUserNotificationCenter center,
            UNNotification notification,
            Action<UNNotificationPresentationOptions> completionHandler)
        {
            _pushNotificationsReceiver.OnMessageReceived(notification.Request.Content.UserInfo, true);
            switch (_showForegroundNotificationsInSystemOptions)
            {
                case ForegroundNotificationOptions.Show:
                    completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound);
                    break;
                case ForegroundNotificationOptions.ShowWithBadge:
                    completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound | UNNotificationPresentationOptions.Badge);
                    break;
                case ForegroundNotificationOptions.DoNotShow:
                    completionHandler(UNNotificationPresentationOptions.None);
                    break;
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
