// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Android.Gms.Extensions;
using Firebase.Messaging;
using Java.IO;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.PushNotifications.Droid.Abstract;

namespace Softeq.XToolkit.PushNotifications.Droid.Services
{
    public sealed class DroidPushNotificationsService : PushNotifications.Abstract.IPushNotificationsService, IDisposable
    {
        private readonly IPushNotificationsConsumer _pushNotificationsConsumer;
        private readonly ILogger _logger;

        public DroidPushNotificationsService(
            IPushNotificationsConsumer pushNotificationsConsumer,
            ILogManager logManager)
        {
            _pushNotificationsConsumer = pushNotificationsConsumer;
            _logger = logManager.GetLogger<DroidPushNotificationsService>();

            XFirebaseMessagingService.OnTokenRefreshed += OnPushTokenRefreshed;
            XFirebaseMessagingService.OnNotificationReceived += OnNotificationReceived;
        }

        public async Task RegisterForPushNotifications()
        {
            var token = await FirebaseMessaging.Instance
                .GetToken()
                .AsAsync<Java.Lang.String>();

            if (token != null)
            {
                _pushNotificationsConsumer.OnPushTokenRefreshed(token.ToString());
            }
        }

        public async Task UnRegisterForPushNotifications()
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
                // TODO
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

        public void Dispose()
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
