// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Android.Gms.Extensions;
using Firebase.Messaging;
using Java.IO;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.PushNotifications.Abstract;
using Softeq.XToolkit.PushNotifications.Droid.Abstract;

namespace Softeq.XToolkit.PushNotifications.Droid.Services
{
    /// <summary>
    ///     Default implementation of <see cref="IPushNotificationsService"/> interface for Android platform.
    /// </summary>
    public sealed class DroidPushNotificationsService : IPushNotificationsService, IDisposable
    {
        private readonly IPushNotificationsConsumer _pushNotificationsConsumer;
        private readonly ILogger _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DroidPushNotificationsService"/> class.
        /// </summary>
        /// <param name="pushNotificationsConsumer">Consumer of the push notification related callbacks.</param>
        /// <param name="logManager">Provides logging.</param>
        public DroidPushNotificationsService(
            IPushNotificationsConsumer pushNotificationsConsumer,
            ILogManager logManager)
        {
            _pushNotificationsConsumer = pushNotificationsConsumer;
            _logger = logManager.GetLogger<DroidPushNotificationsService>();

            XFirebaseMessagingService.OnTokenRefreshed += OnPushTokenRefreshed;
            XFirebaseMessagingService.OnNotificationReceived += OnNotificationReceived;
        }

        /// <inheritdoc />
        public async Task RegisterForPushNotificationsAsync()
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
        public async Task UnregisterForPushNotificationsAsync()
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
