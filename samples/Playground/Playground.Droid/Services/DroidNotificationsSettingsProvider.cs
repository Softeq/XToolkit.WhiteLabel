// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Android.App;
using Android.Content;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Playground.Droid.Views;
using Softeq.XToolkit.PushNotifications;
using Softeq.XToolkit.PushNotifications.Droid;
using Softeq.XToolkit.WhiteLabel.Droid.Providers;

namespace Playground.Droid.Services
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Playground")]
    public class DroidNotificationsSettingsProvider : INotificationsSettingsProvider
    {
        private readonly IContextProvider _contextProvider;

        public DroidNotificationsSettingsProvider(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public string LaunchedFromPushNotificationKey => "launched_from_push_notification";

        public string DefaultChannelId => "toolkit";

        private static string DefaultChannelName => "XToolkit Push Notifications";

        public Dictionary<string, string> GetNotificationChannels(Context context)
        {
            return new Dictionary<string, string>
            {
                { DefaultChannelId, DefaultChannelName },
            };
        }

        public NotificationImportance GetNotificationChannelImportance(string channelId)
        {
            return NotificationImportance.High;
        }

        public void ConfigureNotificationChannel(string channelId, NotificationChannel channel)
        {
            // We don't need custom configuration for notification channels
        }

        public NotificationIntentActivityInfo GetIntentActivityInfoFromPush(PushNotificationModel pushNotification)
        {
            return new NotificationIntentActivityInfo(typeof(SplashActivity), false);
        }

        public void CustomizeNotificationBuilder(
            NotificationCompat.Builder notificationBuilder,
            PushNotificationModel pushNotification,
            int notificationId)
        {
            // notificationBuilder.SetShowWhen(true);
        }

        public PushNotificationStyles GetStylesForNotification(PushNotificationModel pushNotification)
        {
            var style = new NotificationCompat.BigTextStyle().BigText(pushNotification.Body);

            return new PushNotificationStyles(style)
            {
                IconRes = Resource.Drawable.ic_chat,
                IconArgbColor = ContextCompat.GetColor(_contextProvider.AppContext, Resource.Color.colorPrimary),
                Priority = NotificationPriority.High,
            };
        }

        public string GetChannelIdForNotification(PushNotificationModel pushNotification)
        {
            return DefaultChannelId;
        }
    }
}
