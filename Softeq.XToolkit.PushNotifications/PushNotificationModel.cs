// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.PushNotifications
{
    public class PushNotificationModel
    {
        /// <summary>
        ///     Notification title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Notification body - message
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        ///     Value to determine if notification is silent (not visible to the user, for some inner updates)
        ///     On iOS: contains content-available key with value 1
        ///     On Android: does not contain Body
        /// </summary>
        public bool IsSilent { get; set; }

        /// <summary>
        ///     Additional data stored in notification (custom data)
        /// </summary>
        public string AdditionalData { get; set; }

        /// <summary>
        ///     Notification type as string (custom data)
        /// </summary>
        public string Type { get; set; }
    }
}
