// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.PushNotifications.iOS.Abstract;

namespace Softeq.XToolkit.PushNotifications.iOS.Utils
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
