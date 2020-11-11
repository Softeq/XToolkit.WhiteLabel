// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.PushNotifications
{
    public class PushNotificationModel
    {
        /// <summary>
        ///     Gets or sets notification title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        ///     Gets or sets notification body - message.
        /// </summary>
        public string? Body { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the notification is silent (not visible to the user, for some inner updates).
        ///     <para/>
        ///     On iOS: contains content-available key with value 1.
        ///     <para/>
        ///     On Android: does not contain Body.
        /// </summary>
        public bool IsSilent { get; set; }

        /// <summary>
        ///     Gets or sets additional data stored in notification (custom data).
        /// </summary>
        public string AdditionalData { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets notification type as string (custom data).
        /// </summary>
        public string Type { get; set; } = string.Empty;
    }
}
