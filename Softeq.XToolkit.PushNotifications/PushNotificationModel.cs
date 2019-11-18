// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.PushNotifications
{
    public class PushNotificationModel
    {
        /// <summary>
        ///     Notification title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        ///     Notification body - message
        /// </summary>
        public string Body { get; set; } = string.Empty;

        /// <summary>
        ///     Value to determine if notification is silent (not visible to the user, for some inner updates)
        /// </summary>
        public bool IsSilent { get; set; }

        /// <summary>
        ///     Additional data stored in notification (custom data)
        /// </summary>
        public string AdditionalData { get; set; } = string.Empty;

        /// <summary>
        ///     Notification type as string (custom data)
        /// </summary>
        public string Type { get; set; } = string.Empty;
    }
}
