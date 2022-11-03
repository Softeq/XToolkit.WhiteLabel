// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Firebase.Messaging;
using Softeq.XToolkit.PushNotifications.Droid.Abstract;

namespace Softeq.XToolkit.PushNotifications.Droid.Utils
{
    /// <summary>
    ///     Combines multiple consumers into one.
    /// </summary>
    public sealed class CompositePushNotificationsConsumer : IPushNotificationsConsumer
    {
        private readonly IReadOnlyList<IPushNotificationsConsumer> _consumers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CompositePushNotificationsConsumer"/> class.
        /// </summary>
        /// <param name="consumers">
        ///     List of the consumers. Order define the priority of the consumers - first consumer will always have a chance
        ///     to handle push notification, last consumer will have a chance only if all other consumers haven't handled push notification.
        /// </param>
        public CompositePushNotificationsConsumer(params IPushNotificationsConsumer[] consumers)
        {
            _consumers = consumers;
        }

        /// <inheritdoc />
        public bool TryHandleNotification(RemoteMessage message)
        {
            return _consumers
                .Any(consumer => consumer.TryHandleNotification(message));
        }

        /// <inheritdoc />
        public bool TryHandlePushNotificationExtras(Intent intent)
        {
            return _consumers
                .Any(consumer => consumer.TryHandlePushNotificationExtras(intent));
        }

        /// <inheritdoc />
        public void OnPushTokenRefreshed(string token)
        {
            foreach (var consumer in _consumers)
            {
                consumer.OnPushTokenRefreshed(token);
            }
        }

        /// <inheritdoc />
        public Task OnUnregisterFromPushNotifications()
        {
            return Task.WhenAll(_consumers.Select(x => x.OnUnregisterFromPushNotifications()));
        }
    }
}
