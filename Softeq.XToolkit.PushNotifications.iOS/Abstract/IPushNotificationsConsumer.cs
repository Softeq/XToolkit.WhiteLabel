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
        IEnumerable<UNNotificationCategory> GetCategories();

        void WillPresentNotification(
            UNUserNotificationCenter center,
            UNNotification notification,
            Action<UNNotificationPresentationOptions> completionHandler);

        void DidReceiveNotificationResponse(
            UNUserNotificationCenter center,
            UNNotificationResponse response,
            Action completionHandler);

        void OnPushNotificationAuthorizationResult(bool isGranted);

        void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken);

        void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error);

        void DidReceiveRemoteNotification(
            UIApplication application,
            NSDictionary userInfo,
            Action<UIBackgroundFetchResult> completionHandler);

        Task OnUnregisterFromPushNotifications();
    }
}
