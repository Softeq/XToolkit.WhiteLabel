// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Softeq.XToolkit.PushNotifications.iOS.Abstract;
using UIKit;
using UserNotifications;

namespace Softeq.XToolkit.PushNotifications.iOS.Utils
{
    public sealed class CompositePushNotificationsConsumer : IPushNotificationsConsumer
    {
        private readonly IReadOnlyList<IPushNotificationsConsumer> _consumers;

        public CompositePushNotificationsConsumer(params IPushNotificationsConsumer[] consumers)
        {
            _consumers = consumers;
        }

        public UNAuthorizationOptions GetRequiredAuthorizationOptions()
        {
            return _consumers
                .Aggregate(
                    UNAuthorizationOptions.None,
                    (options, consumer) => options | consumer.GetRequiredAuthorizationOptions());
        }

        public IEnumerable<UNNotificationCategory> GetCategories()
        {
            return _consumers.SelectMany(x => x.GetCategories());
        }

        public bool TryPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            return _consumers
                .Any(consumer => consumer.TryPresentNotification(center, notification, completionHandler));
        }

        public bool TryHandleNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            return _consumers
                .Any(consumer => consumer.TryHandleNotificationResponse(center, response, completionHandler));
        }

        public bool TryHandleRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            return _consumers
                .Any(consumer => consumer.TryHandleRemoteNotification(application, userInfo, completionHandler));
        }

        public void OnPushNotificationAuthorizationResult(bool isGranted)
        {
            foreach (var consumer in _consumers)
            {
                consumer.OnPushNotificationAuthorizationResult(isGranted);
            }
        }

        public void OnRegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            foreach (var consumer in _consumers)
            {
                consumer.OnRegisteredForRemoteNotifications(application, deviceToken);
            }
        }

        public void OnFailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            foreach (var consumer in _consumers)
            {
                consumer.OnFailedToRegisterForRemoteNotifications(application, error);
            }
        }

        public Task OnUnregisterFromPushNotifications()
        {
            return Task.WhenAll(_consumers.Select(x => x.OnUnregisterFromPushNotifications()));
        }
    }
}
