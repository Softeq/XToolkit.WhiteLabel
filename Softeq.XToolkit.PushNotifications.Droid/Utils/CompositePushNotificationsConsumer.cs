// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Messaging;
using Softeq.XToolkit.PushNotifications.Droid.Abstract;

namespace Softeq.XToolkit.PushNotifications.Droid.Utils
{
    public sealed class CompositePushNotificationsConsumer : IPushNotificationsConsumer
    {
        private readonly IReadOnlyList<IPushNotificationsConsumer> _consumers;

        public CompositePushNotificationsConsumer(params IPushNotificationsConsumer[] consumers)
        {
            _consumers = consumers;
        }

        public bool TryHandleNotification(RemoteMessage message)
        {
            return _consumers
                .Any(consumer => consumer.TryHandleNotification(message));
        }

        public void OnPushTokenRefreshed(string token)
        {
            foreach (var consumer in _consumers)
            {
                consumer.OnPushTokenRefreshed(token);
            }
        }

        public Task OnUnregisterFromPushNotifications()
        {
            return Task.WhenAll(_consumers.Select(x => x.OnUnregisterFromPushNotifications()));
        }
    }
}
