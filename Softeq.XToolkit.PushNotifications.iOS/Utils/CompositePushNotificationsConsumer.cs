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
        private readonly IPushNotificationsConsumer _defaultConsumer;
        private readonly IReadOnlyList<ConditionalConsumer> _conditionalConsumers;

        public CompositePushNotificationsConsumer(IPushNotificationsConsumer defaultConsumer, IReadOnlyList<ConditionalConsumer> conditionalConsumers)
        {
            _defaultConsumer = defaultConsumer;
            _conditionalConsumers = conditionalConsumers;
        }

        public IEnumerable<UNNotificationCategory> GetCategories()
        {
            return AllConsumers().SelectMany(x => x.GetCategories());
        }

        public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            var consumer = SelectConsumer(notification);

            consumer.WillPresentNotification(center, notification, completionHandler);
        }

        public void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            var consumer = SelectConsumer(response.Notification);

            consumer.DidReceiveNotificationResponse(center, response, completionHandler);
        }

        public void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            var consumer = SelectConsumer(userInfo);

            consumer.DidReceiveRemoteNotification(application, userInfo, completionHandler);
        }

        public void OnPushNotificationAuthorizationResult(bool isGranted)
        {
            foreach (var consumer in AllConsumers())
            {
                consumer.OnPushNotificationAuthorizationResult(isGranted);
            }
        }

        public void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            foreach (var consumer in AllConsumers())
            {
                consumer.RegisteredForRemoteNotifications(application, deviceToken);
            }
        }

        public void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            foreach (var consumer in AllConsumers())
            {
                consumer.FailedToRegisterForRemoteNotifications(application, error);
            }
        }

        public Task OnUnregisterFromPushNotifications()
        {
            return Task.WhenAll(AllConsumers().Select(x => x.OnUnregisterFromPushNotifications()));
        }

        private IPushNotificationsConsumer SelectConsumer(NSDictionary userInfo)
        {
            return _conditionalConsumers
                .FirstOrDefault(x => x.Filter.CanConsume(userInfo))?
                .Consumer ?? _defaultConsumer;
        }

        private IPushNotificationsConsumer SelectConsumer(UNNotification notification)
        {
            return _conditionalConsumers
                .FirstOrDefault(x => x.Filter.CanConsume(notification))?
                .Consumer ?? _defaultConsumer;
        }

        private IEnumerable<IPushNotificationsConsumer> AllConsumers()
        {
            return _conditionalConsumers
                .Select(x => x.Consumer)
                .Append(_defaultConsumer);
        }
    }
}
