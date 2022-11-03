// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;

namespace Softeq.XToolkit.PushNotifications.iOS.Abstract
{
    public interface IPushNotificationsParser
    {
        /// <summary>
        ///     Parse received notification object to <see cref="PushNotificationModel"/>.
        /// </summary>
        /// <param name="userInfo">Dictionary containing notification data.</param>
        /// <returns>Parsed notification.</returns>
        PushNotificationModel Parse(NSDictionary userInfo);
    }
}
