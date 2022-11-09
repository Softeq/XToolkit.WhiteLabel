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
    /// <summary>
    ///     Combines multiple consumers into one.
    /// </summary>
    public sealed class CompositeIosPushNotificationsConsumer : IIosPushNotificationsConsumer
    {
        private readonly IReadOnlyList<IIosPushNotificationsConsumer> _consumers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CompositeIosPushNotificationsConsumer"/> class.
        /// </summary>
        /// <param name="consumers">
        ///     List of the consumers. Order define the priority of the consumers - first consumer will always have a chance
        ///     to handle push notification, last consumer will have a chance only if all other consumers haven't handled push notification.
        /// </param>
        public CompositeIosPushNotificationsConsumer(params IIosPushNotificationsConsumer[] consumers)
        {
            _consumers = consumers;
        }

        /// <inheritdoc />
        public UNAuthorizationOptions GetRequiredAuthorizationOptions()
        {
            return _consumers
                .Aggregate(
                    UNAuthorizationOptions.None,
                    (options, consumer) => options | consumer.GetRequiredAuthorizationOptions());
        }

        /// <inheritdoc />
        public IEnumerable<UNNotificationCategory> GetCategories()
        {
            return _consumers.SelectMany(x => x.GetCategories());
        }

        /// <inheritdoc />
        public bool TryPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            return _consumers
                .Any(consumer => consumer.TryPresentNotification(center, notification, completionHandler));
        }

        /// <inheritdoc />
        public bool TryHandleNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            return _consumers
                .Any(consumer => consumer.TryHandleNotificationResponse(center, response, completionHandler));
        }

        /// <inheritdoc />
        public bool TryHandleRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            return _consumers
                .Any(consumer => consumer.TryHandleRemoteNotification(application, userInfo, completionHandler));
        }

        /// <inheritdoc />
        public void OnPushNotificationAuthorizationResult(bool isGranted)
        {
            foreach (var consumer in _consumers)
            {
                consumer.OnPushNotificationAuthorizationResult(isGranted);
            }
        }

        /// <inheritdoc />
        public void OnRegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            foreach (var consumer in _consumers)
            {
                consumer.OnRegisteredForRemoteNotifications(application, deviceToken);
            }
        }

        /// <inheritdoc />
        public void OnFailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            foreach (var consumer in _consumers)
            {
                consumer.OnFailedToRegisterForRemoteNotifications(application, error);
            }
        }

        /// <inheritdoc />
        public Task OnUnregisterFromPushNotifications()
        {
            return Task.WhenAll(_consumers.Select(x => x.OnUnregisterFromPushNotifications()));
        }
    }
}
