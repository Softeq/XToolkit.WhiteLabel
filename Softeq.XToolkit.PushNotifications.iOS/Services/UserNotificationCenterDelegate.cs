// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.PushNotifications.iOS.Abstract;
using UserNotifications;

namespace Softeq.XToolkit.PushNotifications.iOS.Services
{
    internal sealed class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        private readonly IPushNotificationsConsumer _pushNotificationsConsumer;
        private readonly ILogger _logger;

        public UserNotificationCenterDelegate(
            IPushNotificationsConsumer pushNotificationsConsumer,
            ILogManager logManager)
        {
            _pushNotificationsConsumer = pushNotificationsConsumer;
            _logger = logManager.GetLogger<UserNotificationCenterDelegate>();
        }

        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            if (_pushNotificationsConsumer.TryPresentNotification(center, notification, completionHandler))
            {
                return;
            }

            _logger.Warn("Notification have not been presented by consumer, invoking default handler");
            completionHandler.Invoke(UNNotificationPresentationOptions.None);
        }

        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            if (_pushNotificationsConsumer.TryHandleNotificationResponse(center, response, completionHandler))
            {
                return;
            }

            _logger.Warn("Notification response have not been handled by consumer, invoking default handler");
            completionHandler.Invoke();
        }
    }
}
