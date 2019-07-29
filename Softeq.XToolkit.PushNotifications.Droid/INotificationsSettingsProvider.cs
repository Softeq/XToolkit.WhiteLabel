// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;

namespace Softeq.XToolkit.PushNotifications.Droid
{
    public interface INotificationsSettingsProvider
    {
        /// <summary>
        ///     Provides Id of the default notification channel
        /// </summary>
        string DefaultChannelId { get; }

        /// <summary>
        ///     Obtains a dictionary of notification channles where the key is channel id and the value is channel name
        /// </summary>
        /// <param name="context">
        ///     Context for obtaining channel names from resources (Names should be obtained from resources for
        ///     localization support)
        /// </param>
        /// <returns>Dictionary of channel ids and names</returns>
        Dictionary<string, string> GetNotificationChannels(Context context);

        /// <summary>
        ///     Obtains notification channel importance level by it's id
        /// </summary>
        /// <param name="channelId">Notification channel id</param>
        /// <returns>Notification channel importance</returns>
        NotificationImportance GetNotificationChannelImportance(string channelId);

        /// <summary>
        ///     Obtains channel id for specific notification
        /// </summary>
        /// <param name="pushNotification">Push notification data</param>
        /// <returns>Channel id string</returns>
        string GetChannelIdForNotification(PushNotificationModel pushNotification);

        /// <summary>
        ///     Obtains styles for displaying the given push notification in system. Provide different ids for notifications if you do not
        ///     want them to replace each other
        /// </summary>
        /// <param name="pushNotification"></param>
        /// <returns></returns>
        PushNotificationStyles GetStylesForNotification(PushNotificationModel pushNotification);

        /// <summary>
        ///     Obtains type of the Start Activity which will be opened by tap on the given notification
        /// </summary>
        /// <param name="pushNotification">Push notification data</param>
        /// <returns>Start Activity type</returns>
        Type GetStartActivityTypeFromPush(PushNotificationModel pushNotification);
    }
}
