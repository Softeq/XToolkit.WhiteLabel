// Developed by Softeq Development Corporation
// http://www.softeq.com

using Firebase.Messaging;

namespace Softeq.XToolkit.PushNotifications.Droid.Abstract
{
    public interface IConsumerFilter
    {
        bool CanConsume(RemoteMessage message);
    }
}
