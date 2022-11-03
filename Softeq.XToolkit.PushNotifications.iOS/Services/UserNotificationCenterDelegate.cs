// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.PushNotifications.iOS.Abstract;
using UserNotifications;

namespace Softeq.XToolkit.PushNotifications.iOS.Services
{
    internal sealed class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        private readonly IPushNotificationsConsumer _pushNotificationsConsumer;

        public UserNotificationCenterDelegate(IPushNotificationsConsumer pushNotificationsConsumer)
        {
            _pushNotificationsConsumer = pushNotificationsConsumer;
        }

        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            if (!_pushNotificationsConsumer.TryPresentNotification(center, notification, completionHandler))
            {
                // TODO
                completionHandler.Invoke(UNNotificationPresentationOptions.None);
            }
        }

        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            if (!_pushNotificationsConsumer.TryHandleNotificationResponse(center, response, completionHandler))
            {
                // TODO
                completionHandler.Invoke();
            }
        }
    }
}
