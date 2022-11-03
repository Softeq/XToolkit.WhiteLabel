// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Softeq.XToolkit.PushNotifications.iOS.Abstract;
using UIKit;
using UserNotifications;

namespace Softeq.XToolkit.PushNotifications.iOS.Services
{
    public class IosPushNotificationService : PushNotifications.Abstract.IPushNotificationsService, IPushNotifficationAppDelegate
    {
        private readonly IPushNotificationsConsumer _pushNotificationsConsumer;

        public IosPushNotificationService(IPushNotificationsConsumer pushNotificationsConsumer)
        {
            _pushNotificationsConsumer = pushNotificationsConsumer;

            UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate(pushNotificationsConsumer);
            UNUserNotificationCenter.Current.SetNotificationCategories(new NSSet<UNNotificationCategory>(pushNotificationsConsumer.GetCategories().ToArray()));
        }

        public async Task RegisterForPushNotifications()
        {
            var requiredAuthorizationOptions = _pushNotificationsConsumer.GetRequiredAuthorizationOptions();
            var (isGranted, _) = await UNUserNotificationCenter.Current.RequestAuthorizationAsync(requiredAuthorizationOptions);
            _pushNotificationsConsumer.OnPushNotificationAuthorizationResult(isGranted);

            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            });
        }

        public async Task UnRegisterForPushNotifications()
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.UnregisterForRemoteNotifications();
            });

            await _pushNotificationsConsumer.OnUnregisterFromPushNotifications();
        }

        public void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            _pushNotificationsConsumer.OnRegisteredForRemoteNotifications(application, deviceToken);
        }

        public void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            _pushNotificationsConsumer.OnFailedToRegisterForRemoteNotifications(application, error);
        }

        public void DidReceiveRemoteNotification(
            UIApplication application,
            NSDictionary userInfo,
            Action<UIBackgroundFetchResult> completionHandler)
        {
            if (!_pushNotificationsConsumer.TryHandleRemoteNotification(application, userInfo, completionHandler))
            {
                // TODO
                completionHandler.Invoke(UIBackgroundFetchResult.Failed);
            }
        }
    }
}
