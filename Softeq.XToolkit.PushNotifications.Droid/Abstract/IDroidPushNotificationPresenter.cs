// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;

namespace Softeq.XToolkit.PushNotifications.Droid.Abstract
{
    /// <summary>
    ///     Basic interface for handling the push notification presentation.
    /// </summary>
    public interface IDroidPushNotificationPresenter
    {
        /// <summary>
        ///     Presents the notification, if needed.
        /// </summary>
        /// <param name="notificationModel">Parsed notification model.</param>
        /// <param name="notificationData">Remote notification data payload.</param>
        /// <param name="isInForeground"><see langword="true"/> if application is currently in foreground, <see langword="false"/> otherwise.</param>
        void Present(
            PushNotificationModel notificationModel,
            IDictionary<string, string> notificationData,
            bool isInForeground);
    }
}
