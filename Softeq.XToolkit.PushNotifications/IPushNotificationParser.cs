// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.PushNotifications
{
    public interface IPushNotificationParser
    {
        /// <summary>
        /// Parse received notification object to PushNotificationModel.
        /// Received object is RemoteMessage or Bundle for Android; NSDictionary for iOS
        /// </summary>
        /// <param name="pushNotificationData">Push notification object</param>
        /// <returns>Parsed notification</returns>
        PushNotificationModel Parse(object pushNotificationData);
    }
}