// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using TaskStackBuilder = Android.App.TaskStackBuilder;

namespace Softeq.XToolkit.PushNotifications.Droid
{
    internal static class NotificationsHelper
    {
        private static INotificationsSettingsProvider _notificationsSettings;

        public static void Init(INotificationsSettingsProvider notificationsSettings)
        {
            _notificationsSettings = notificationsSettings;
        }

        // You should call Init prior to using this method
        public static void CreateNotificationChannels(Context context)
        {
            if (_notificationsSettings == null)
            {
                return;
            }

            // Create the NotificationChannel, but only on API 26+ because
            // the NotificationChannel class is new and not in the support library
            if (context != null && Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channels = _notificationsSettings.GetNotificationChannels(context);
                var channelsList = new List<NotificationChannel>();

                foreach (var channel in channels)
                {
                    var channelId = channel.Key;
                    var channelName = channel.Value;
                    var channelImportance = _notificationsSettings.GetNotificationChannelImportance(channelId);
                    var notificationChannel = new NotificationChannel(channelId, channelName, channelImportance);
                    _notificationsSettings.ConfigureNotificationChannel(channelId, notificationChannel);
                    channelsList.Add(notificationChannel);
                }

                if (channelsList.Any())
                {
                    NotificationManager.FromContext(context).CreateNotificationChannels(channelsList);
                }
            }
        }

        // You should call Init prior to using this method
        public static void CreateNotification(
            Context context,
            PushNotificationModel pushNotification,
            IDictionary<string, string> notificationData)
        {
            if (context == null || _notificationsSettings == null)
            {
                return;
            }

            var channelId = _notificationsSettings.GetChannelIdForNotification(pushNotification);
            if (string.IsNullOrEmpty(channelId))
            {
                channelId = _notificationsSettings.DefaultChannelId;
            }

            var title = string.IsNullOrEmpty(pushNotification.Title) ? GetApplicationName(context) : pushNotification.Title;
            var message = pushNotification.Body;

            var styles = _notificationsSettings.GetStylesForNotification(pushNotification);

            var notificationBuilder = new NotificationCompat.Builder(context, channelId)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetPriority((int) styles.Priority)
                .SetAutoCancel(styles.AutoCancel)
                .SetStyle(styles.Style)
                .SetSound(styles.SoundUri)
                .SetSmallIcon(styles.IconRes)
                .SetColor(styles.IconArgbColor);

            var intentActivityInfo = _notificationsSettings.GetIntentActivityInfoFromPush(pushNotification);
            var intent = new Intent(context, intentActivityInfo.ActivityType);

            if (notificationData != null)
            {
                foreach (var (key, value) in notificationData)
                {
                    intent.PutExtra(key, value);
                }
            }

            var pendingIntent = CreatePendingIntent(context, intent, intentActivityInfo.DoCreateParentStack);
            notificationBuilder.SetContentIntent(pendingIntent);

            _notificationsSettings.CustomizeNotificationBuilder(notificationBuilder, pushNotification, styles.Id);

            NotificationManagerCompat.From(context).Notify(styles.Id, notificationBuilder.Build());
        }

        private static PendingIntent CreatePendingIntent(Context context, Intent intent, bool withParentStack)
        {
            if (withParentStack)
            {
                var stackBuilder = TaskStackBuilder.Create(context);
                stackBuilder.AddNextIntentWithParentStack(intent);
                return stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);
            }
            else
            {
                intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                return PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.UpdateCurrent);
            }
        }

        private static string GetApplicationName(Context context)
        {
            var applicationInfo = context.ApplicationInfo;
            var stringId = applicationInfo.LabelRes;
            return stringId == 0 ? applicationInfo.NonLocalizedLabel.ToString() : context.GetString(stringId);
        }
    }
}
