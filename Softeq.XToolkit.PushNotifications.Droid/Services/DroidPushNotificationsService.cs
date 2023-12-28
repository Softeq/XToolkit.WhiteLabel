// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using AndroidX.Core.App;
using Firebase.Messaging;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.PushNotifications.Abstract;
using Softeq.XToolkit.PushNotifications.Droid.Abstract;

namespace Softeq.XToolkit.PushNotifications.Droid.Services
{
    /// <summary>
    ///     Default implementation of <see cref="IPushNotificationsService"/> and <see cref="IActivityLauncherDelegate"/>
    ///     interfaces for Android platform. Handles all interactions with the platform, related to push notifications.
    /// </summary>
    public sealed class DroidPushNotificationsService :
        IPushNotificationsService, IActivityLauncherDelegate, IDroidFirebaseMessagingHandler, IDisposable
    {
        private readonly IDroidPushNotificationsConsumer _pushNotificationsConsumer;
        private readonly ILogger _logger;

        private readonly SemaphoreSlim _registrationSemaphore = new SemaphoreSlim(1, 1);

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
        }

        ~DroidPushNotificationsService()
        {
            Dispose(false);
        }

        private bool IsRegistered { get; set; }

        /// <inheritdoc />
        public async Task RegisterAsync()
        {
            await _registrationSemaphore
                .WaitAsync()
                .ConfigureAwait(false);

            try
            {
                if (IsRegistered)
                {
                    return;
                }

                var token = await FirebaseMessaging.Instance
                    .GetToken()
                    .AsAsync<Java.Lang.String>()
                    .ConfigureAwait(false);

                IsRegistered = true;

                _pushNotificationsConsumer.OnPushTokenRefreshed(token.ToString());
            }
            finally
            {
                _registrationSemaphore.Release();
            }
        }

        /// <inheritdoc />
        public async Task UnregisterAsync()
        {
            await _registrationSemaphore
                .WaitAsync()
                .ConfigureAwait(false);

            try
            {
                if (!IsRegistered)
                {
                    return;
                }

                await FirebaseMessaging.Instance
                    .DeleteToken()
                    .AsAsync()
                    .ConfigureAwait(false); // Throws Java.IOException if there's no Internet Connection

                IsRegistered = false;

                await _pushNotificationsConsumer
                    .OnUnregisterFromPushNotifications()
                    .ConfigureAwait(false);
            }
            finally
            {
                _registrationSemaphore.Release();
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _registrationSemaphore.Dispose();
            }
        }

        /// <inheritdoc />
        void IDroidFirebaseMessagingHandler.OnNotificationReceived(RemoteMessage message)
        {
            if (!_pushNotificationsConsumer.TryHandleNotification(message))
            {
                _logger.Warn("Notification have not been handled by consumer");
            }
        }

        // FCM has an auto-init feature enabled by default. It means that even if token has been deleted - it will be
        // re-generated automatically, even if we are not subscribed for push-notifications. To preserve consistent behaviour
        // for consumers that push token is provided only after subscription, we need to keep track of subscription status.
        // NOTE: disabling auto-init might not be the option, as it also requires disabling Firebase Analytics
        // https://firebase.google.com/docs/cloud-messaging/android/client#prevent-auto-init

        /// <inheritdoc />
        void IDroidFirebaseMessagingHandler.OnPushTokenRefreshed(string token)
        {
            _registrationSemaphore.Wait();

            try
            {
                if (IsRegistered)
                {
                    _pushNotificationsConsumer.OnPushTokenRefreshed(token);
                }
            }
            finally
            {
                _registrationSemaphore.Release();
            }
        }
    }
}
