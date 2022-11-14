// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;

namespace Softeq.XToolkit.PushNotifications.iOS.Abstract
{
    public interface IIosPushNotificationsParser
    {
        /// <summary>
        ///     Parse received notification object to <see cref="PushNotificationModel"/>.
        /// </summary>
        /// <param name="userInfo">Dictionary containing notification data.</param>
        /// <param name="parsedPushNotificationModel">Parsed notification.</param>
        /// <returns><see langword="true"/> if notification has been parsed, <see langword="false"/> otherwise.</returns>
        bool TryParse(NSDictionary userInfo, out PushNotificationModel parsedPushNotificationModel);
    }
}
