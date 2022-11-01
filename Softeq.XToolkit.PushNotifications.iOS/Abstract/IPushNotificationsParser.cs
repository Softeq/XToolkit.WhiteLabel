// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;

namespace Softeq.XToolkit.PushNotifications.iOS.Abstract
{
    public interface IPushNotificationsParser
    {
        PushNotificationModel Parse(NSDictionary userInfo);
    }
}
