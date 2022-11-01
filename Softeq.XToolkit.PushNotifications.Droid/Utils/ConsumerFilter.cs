// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Firebase.Messaging;
using Softeq.XToolkit.PushNotifications.Droid.Abstract;

namespace Softeq.XToolkit.PushNotifications.Droid.Utils
{
    public sealed class ConsumerFilter : IConsumerFilter
    {
        private readonly Func<RemoteMessage, bool> _delegate;

        public ConsumerFilter(Func<RemoteMessage, bool> @delegate)
        {
            _delegate = @delegate;
        }

        public bool CanConsume(RemoteMessage message) => _delegate.Invoke(message);
    }
}
