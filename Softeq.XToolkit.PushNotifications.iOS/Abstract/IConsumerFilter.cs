// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;
using UserNotifications;

namespace Softeq.XToolkit.PushNotifications.iOS.Abstract
{
    public interface IConsumerFilter
    {
        bool CanConsume(UNNotification notification);

        bool CanConsume(NSDictionary userInfo);
    }
}
