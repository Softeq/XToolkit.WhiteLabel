// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.PushNotifications.Droid.Abstract;

namespace Softeq.XToolkit.PushNotifications.Droid.Utils
{
    public sealed class ConditionalConsumer
    {
        public ConditionalConsumer(IPushNotificationsConsumer consumer, IConsumerFilter filter)
        {
            Consumer = consumer;
            Filter = filter;
        }

        public IPushNotificationsConsumer Consumer { get; }

        public IConsumerFilter Filter { get; }
    }
}
