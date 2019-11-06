// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;

namespace Softeq.XToolkit.PushNotifications.Droid
{
    public class NotificationsHelper
    {
        public static void CreateNotificationChannels(Context context, INotificationsSettingsProvider notificationsSettings)
        {
            // Create the NotificationChannel, but only on API 26+ because
            // the NotificationChannel class is new and not in the support library
            if (context != null && Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channels = notificationsSettings.GetNotificationChannels(context);
                var channelsList = new List<NotificationChannel>();

                foreach (var channel in channels)
                {
                    var channelId = channel.Key;
                    var channelName = channel.Value;
                    var channelImportance = notificationsSettings.GetNotificationChannelImportance(channelId);
                    var notificationChannel = new NotificationChannel(channelId, channelName, channelImportance);
                    notificationsSettings.ConfigureNotificationChannel(channelId, notificationChannel);
                    channelsList.Add(notificationChannel);
                }

                if (channelsList.Any())
                {
                    var notificationManager = NotificationManager.FromContext(context);
                    notificationManager.CreateNotificationChannels(channelsList);
                }
            }
        }

        public static void CreateNotification(
            Context context,
            PushNotificationModel pushNotification,
            IDictionary<string, string> notificationData,
            INotificationsSettingsProvider notificationsSettings)
        {
            if (context == null)
            {
                return;
            }

            var channelId = notificationsSettings.GetChannelIdForNotification(pushNotification);
            if (string.IsNullOrEmpty(channelId))
            {
                channelId = notificationsSettings.DefaultChannelId;
            }

            var title = string.IsNullOrEmpty(pushNotification.Title) ? GetApplicationName(context) : pushNotification.Title;
            var message = pushNotification.Body;

            var styles = notificationsSettings.GetStylesForNotification(pushNotification);

            var notificationBuilder = new NotificationCompat.Builder(context, channelId)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetAutoCancel(styles.AutoCancel)
                .SetSound(styles.SoundUri)
                .SetPriority((int) styles.Priority)
                .SetStyle(styles.Style)
                .SetSmallIcon(styles.IconRes)
                .SetColor(styles.IconArgbColor);

            var startActivityType = notificationsSettings.GetStartActivityTypeFromPush(pushNotification);
            var intent = new Intent(context, startActivityType);
            intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);

            if (notificationData != null)
            {
                foreach (var (key, value) in notificationData)
                {
                    intent.PutExtra(key, value);
                }
            }

            var pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.UpdateCurrent);
            notificationBuilder.SetContentIntent(pendingIntent);

            notificationBuilder = notificationsSettings.CustomizeNotificationBuilder(notificationBuilder, pushNotification);

            NotificationManager.FromContext(context).Notify(styles.Id, notificationBuilder.Build());
        }

        private static string GetApplicationName(Context context)
        {
            var applicationInfo = context.ApplicationInfo;
            var stringId = applicationInfo.LabelRes;
            return stringId == 0 ? applicationInfo.NonLocalizedLabel.ToString() : context.GetString(stringId);
        }
    }
}
