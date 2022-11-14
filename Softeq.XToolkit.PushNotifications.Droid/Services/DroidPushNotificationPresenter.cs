// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Android.App;
using Softeq.XToolkit.PushNotifications.Droid.Abstract;

namespace Softeq.XToolkit.PushNotifications.Droid.Services
{
    /// <summary>
    ///     Default implementation of <see cref="IDroidPushNotificationPresenter"/> interface.
    /// </summary>
    public sealed class DroidPushNotificationPresenter : IDroidPushNotificationPresenter
    {
        private readonly ForegroundNotificationOptions _foregroundNotificationOptions;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DroidPushNotificationPresenter"/> class.
        /// </summary>
        /// <param name="notificationsSettingsProvider">Provides settings for notification construction.</param>
        /// <param name="foregroundNotificationOptions">Defines how push notification should be presented in foreground.</param>
        public DroidPushNotificationPresenter(
            INotificationsSettingsProvider notificationsSettingsProvider,
            ForegroundNotificationOptions foregroundNotificationOptions)
        {
            _foregroundNotificationOptions = foregroundNotificationOptions;

            NotificationsHelper.Init(notificationsSettingsProvider);
            NotificationsHelper.CreateNotificationChannels(Application.Context);
        }

        /// <inheritdoc />
        public void Present(
            PushNotificationModel notificationModel,
            IDictionary<string, string> notificationData,
            bool isInForeground)
        {
            if (!notificationModel.IsSilent && (_foregroundNotificationOptions.ShouldShow() || !isInForeground))
            {
                NotificationsHelper.CreateNotification(Application.Context, notificationModel, notificationData);
            }
        }
    }
}
