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
        private readonly IPushNotificationsConsumer _defaultConsumer;
        private readonly IReadOnlyList<ConditionalConsumer> _conditionalConsumers;

        public CompositePushNotificationsConsumer(
            IPushNotificationsConsumer defaultConsumer,
            IReadOnlyList<ConditionalConsumer> conditionalConsumers)
        {
            _defaultConsumer = defaultConsumer;
            _conditionalConsumers = conditionalConsumers;
        }

        public void OnNotificationReceived(RemoteMessage message)
        {
            var consumer = SelectConsumer(message);

            consumer.OnNotificationReceived(message);
        }

        public void OnPushTokenRefreshed(string token)
        {
            foreach (var consumer in AllConsumers())
            {
                consumer.OnPushTokenRefreshed(token);
            }
        }

        public Task OnUnregisterFromPushNotifications()
        {
            return Task.WhenAll(AllConsumers().Select(x => x.OnUnregisterFromPushNotifications()));
        }

        private IPushNotificationsConsumer SelectConsumer(RemoteMessage message)
        {
            return _conditionalConsumers
                .FirstOrDefault(x => x.Filter.CanConsume(message))?
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
