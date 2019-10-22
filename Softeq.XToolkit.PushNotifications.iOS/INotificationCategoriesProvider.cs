// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using UserNotifications;

namespace Softeq.XToolkit.PushNotifications.iOS
{
    public interface INotificationCategoriesProvider
    {
        IList<UNNotificationCategory> NotificationCategories { get; }
        IList<UNNotificationAction> NotificationActions { get; }

        void HandlePushNotificationCustomAction(PushNotificationModel pushNotification, string actionId, string textInput);
    }
}
