// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using UserNotifications;

namespace Softeq.XToolkit.PushNotifications.iOS.Abstract
{
    public interface IPushNotificationsConsumer
    {
        UNAuthorizationOptions GetRequiredAuthorizationOptions();

        IEnumerable<UNNotificationCategory> GetCategories();

        bool TryPresentNotification(
            UNUserNotificationCenter center,
            UNNotification notification,
            Action<UNNotificationPresentationOptions> completionHandler);

        bool TryHandleNotificationResponse(
            UNUserNotificationCenter center,
            UNNotificationResponse response,
            Action completionHandler);

        bool TryHandleRemoteNotification(
            UIApplication application,
            NSDictionary userInfo,
            Action<UIBackgroundFetchResult> completionHandler);

        void OnPushNotificationAuthorizationResult(bool isGranted);

        void OnRegisteredForRemoteNotifications(UIApplication application, NSData deviceToken);

        void OnFailedToRegisterForRemoteNotifications(UIApplication application, NSError error);

        Task OnUnregisterFromPushNotifications();
    }
}
