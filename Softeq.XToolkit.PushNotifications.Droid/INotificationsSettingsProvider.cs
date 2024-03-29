﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Android.App;
using Android.Content;
using AndroidX.Core.App;

namespace Softeq.XToolkit.PushNotifications.Droid
{
    public interface INotificationsSettingsProvider
    {
        /// <summary>
        ///     Gets Id of the default notification channel.
        /// </summary>
        string DefaultChannelId { get; }

        /// <summary>
        ///     Gets key for saving boolean value to the Activity intent.
        ///     This value will indicate, if the app was started from Push Notification or not.
        /// </summary>
        string LaunchedFromPushNotificationKey { get; }

        /// <summary>
        ///     Obtains a dictionary of notification channels where the key is channel id and the value is channel name.
        /// </summary>
        /// <remarks>
        ///     if you are using "notification" notifications,
        ///     Firebase will use a separate channel for them when received in Background.
        /// </remarks>
        /// <param name="context">
        ///     Context for obtaining channel names from resources.
        ///     <para/>
        ///     Names should be obtained from resources for
        ///     localization support, this list of channels will be automatically recreated when language changes.
        /// </param>
        /// <returns>Dictionary of channel Ids and Names.</returns>
        Dictionary<string, string> GetNotificationChannels(Context context);

        /// <summary>
        ///     Obtains notification channel importance level by it's id.
        /// </summary>
        /// <param name="channelId">Notification channel id.</param>
        /// <returns>Notification channel importance.</returns>
        NotificationImportance GetNotificationChannelImportance(string channelId);

        /// <summary>
        ///     You can add some custom configuration for a created Notification Channel (like description, sound,
        ///     turn off badges, create and set group - <see cref="NotificationChannelsHelper"/> to create a group, etc.).
        /// </summary>
        /// <remarks>This method will only be called on API 26+.</remarks>
        /// <param name="channelId">Channel Id string.</param>
        /// <param name="channel">
        ///     A NotificationChannel that will be registered in the system. Contains already set channelId,
        ///     name (received from <see cref="GetNotificationChannels"/>)
        ///     and importance (provided by <see cref="GetNotificationChannelImportance"/>).
        /// </param>
        void ConfigureNotificationChannel(string channelId, NotificationChannel channel);

        /// <summary>
        ///     Obtains channel id for specific notification.
        /// </summary>
        /// <param name="pushNotification">Push notification data.</param>
        /// <returns>Channel id string.</returns>
        string GetChannelIdForNotification(PushNotificationModel pushNotification);

        /// <summary>
        ///     Obtains styles for displaying the given push notification in system. Provide different ids for notifications if you do not
        ///     want them to replace each other.
        /// </summary>
        /// <param name="pushNotification">Push notification data.</param>
        /// <returns>Styles.</returns>
        PushNotificationStyles GetStylesForNotification(PushNotificationModel pushNotification);

        /// <summary>
        ///     You can customize how push notification will be shown (apart from ContentTitle, ContentText, channelId,
        ///     styles set in GetStylesForNotification and content intent). For instance, you can use SetNumber to change badge
        ///     value increment on Android 26+; add Action buttons; add groups and create additional notifications - like group
        ///     summary notification; add progress bar and later update for this notificationId; or simply save notificationId; etc.
        /// </summary>
        /// <param name="notificationBuilder">Already created notification builder that can be further customized.</param>
        /// <param name="pushNotification">Push notification data.</param>
        /// <param name="notificationId"></param>
        void CustomizeNotificationBuilder(
            NotificationCompat.Builder notificationBuilder,
            PushNotificationModel pushNotification,
            int notificationId);

        /// <summary>
        ///     Provides info about the Activity which will be opened by tap on the given notification.
        /// </summary>
        /// <param name="pushNotification">Push notification data.</param>
        /// <returns>Intent Activity info (type and whether parent stack should be created).</returns>
        NotificationIntentActivityInfo GetIntentActivityInfoFromPush(PushNotificationModel pushNotification);
    }
}
