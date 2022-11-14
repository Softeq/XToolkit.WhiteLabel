// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using AndroidX.Core.App;
using Firebase.Messaging;
using Java.IO;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.PushNotifications.Abstract;
using Softeq.XToolkit.PushNotifications.Droid.Abstract;

namespace Softeq.XToolkit.PushNotifications.Droid.Services
{
    /// <summary>
    ///     Default implementation of <see cref="IPushNotificationsService"/> and <see cref="IActivityLauncherDelegate"/>
    ///     interfaces for Android platform. Handles all interactions with the platform, related to push notifications.
    /// </summary>
    public sealed class DroidPushNotificationsService : IPushNotificationsService, IActivityLauncherDelegate, IDisposable
    {
        private readonly IDroidPushNotificationsConsumer _pushNotificationsConsumer;
        private readonly ILogger _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DroidPushNotificationsService"/> class.
        /// </summary>
        /// <param name="pushNotificationsConsumer">Consumer of the push notification related callbacks.</param>
        /// <param name="logManager">Provides logging.</param>
        public DroidPushNotificationsService(
            IDroidPushNotificationsConsumer pushNotificationsConsumer,
            ILogManager logManager)
        {
            _pushNotificationsConsumer = pushNotificationsConsumer;
            _logger = logManager.GetLogger<DroidPushNotificationsService>();

            XFirebaseMessagingService.OnTokenRefreshed += OnPushTokenRefreshed;
            XFirebaseMessagingService.OnNotificationReceived += OnNotificationReceived;
        }

        /// <inheritdoc />
        public async Task RegisterAsync()
        {
            var token = await FirebaseMessaging.Instance
                .GetToken()
                .AsAsync<Java.Lang.String>();

            if (token != null)
            {
                _pushNotificationsConsumer.OnPushTokenRefreshed(token.ToString());
            }
        }

        /// <inheritdoc />
        public async Task UnregisterAsync()
        {
            try
            {
                await FirebaseMessaging.Instance
                    .DeleteToken()
                    .AsAsync(); // Throws Java.IOException if there's no Internet Connection
                await _pushNotificationsConsumer.OnUnregisterFromPushNotifications();
            }
            catch (IOException e)
            {
                _logger.Warn($"Firebase DeleteToken failed: {e.Message}");
            }
        }

        /// <inheritdoc />
        public void ClearAllNotifications()
        {
            NotificationManagerCompat.From(Application.Context).CancelAll();
        }

        /// <inheritdoc />
        public bool TryHandleLaunchIntent(Intent? intent)
        {
            return intent != null && _pushNotificationsConsumer.TryHandlePushNotificationIntent(intent);
        }

        private void OnNotificationReceived(RemoteMessage message)
        {
            if (!_pushNotificationsConsumer.TryHandleNotification(message))
            {
                _logger.Warn("Notification have not been handled by consumer");
            }
        }

        private void OnPushTokenRefreshed(string token)
        {
            _pushNotificationsConsumer.OnPushTokenRefreshed(token);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                XFirebaseMessagingService.OnTokenRefreshed -= OnPushTokenRefreshed;
                XFirebaseMessagingService.OnNotificationReceived -= OnNotificationReceived;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DroidPushNotificationsService()
        {
            Dispose(false);
        }
    }
}
