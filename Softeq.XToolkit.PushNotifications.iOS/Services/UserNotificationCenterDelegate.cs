// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.PushNotifications.iOS.Abstract;
using UserNotifications;

namespace Softeq.XToolkit.PushNotifications.iOS.Services
{
    public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        private readonly IPushNotificationsConsumer _pushNotificationsConsumer;

        public UserNotificationCenterDelegate(IPushNotificationsConsumer pushNotificationsConsumer)
        {
            _pushNotificationsConsumer = pushNotificationsConsumer;
        }

        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            _pushNotificationsConsumer.WillPresentNotification(center, notification, completionHandler);
        }

        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            _pushNotificationsConsumer.DidReceiveNotificationResponse(center, response, completionHandler);
        }
    }
}
