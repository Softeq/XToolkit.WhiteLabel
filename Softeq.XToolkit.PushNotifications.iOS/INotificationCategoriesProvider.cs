// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using UserNotifications;

namespace Softeq.XToolkit.PushNotifications.iOS
{
    public interface INotificationCategoriesProvider
    {
        /// <summary>
        /// List of Notification Categories that need to be registered for the app
        /// </summary>
        IList<UNNotificationCategory> NotificationCategories { get; }

        /// <summary>
        /// Handle the situation when a custom action of a push notification was invoked by the user
        /// This includes Dismiss action for categories with CustomDismissAction option
        /// </summary>
        /// <param name="pushNotification">Push notification model</param>
        /// <param name="actionId">The identifier of the action invoked</param>
        /// <param name="textInput">Text provided by the user for UNTextInputNotificationAction, the value is null for simple UNNotificationAction</param>
        void HandlePushNotificationCustomAction(PushNotificationModel pushNotification, string actionId, string textInput);
    }
}
