﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Android.Content;
using AndroidX.Lifecycle;
using Firebase.Messaging;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.PushNotifications.Abstract;
using Softeq.XToolkit.PushNotifications.Droid.Abstract;

namespace Softeq.XToolkit.PushNotifications.Droid.Services
{
    /// <summary>
    ///     Default implementation of <see cref="IDroidPushNotificationsConsumer"/> interface for Android platform.
    /// </summary>
    public sealed class DroidPushNotificationsConsumer : IDroidPushNotificationsConsumer
    {
        private readonly IPushTokenStorageService _pushTokenStorageService;
        private readonly IDroidPushNotificationsParser _pushNotificationsParser;
        private readonly IPushNotificationsHandler _pushNotificationsHandler;
        private readonly IRemotePushNotificationsService _remotePushNotificationsService;
        private readonly IDroidPushNotificationPresenter _pushNotificationPresenter;
        private readonly IPushTokenSynchronizer _pushTokenSynchronizer;
        private readonly ILogger _logger;

        private readonly AppLifecycleObserver _lifecycleObserver;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DroidPushNotificationsConsumer"/> class.
        /// </summary>
        /// <param name="pushNotificationsParser">Parses remote messages into common format.</param>
        /// <param name="pushTokenStorageService">Stores push notification token and it's status.</param>
        /// <param name="pushNotificationsHandler">Handles received push notifications events.</param>
        /// <param name="remotePushNotificationsService">Propagates push notification token to remote server.</param>
        /// <param name="pushNotificationPresenter">Responsible for presenting push notification.</param>
        /// <param name="logManager">Provides logging.</param>
        public DroidPushNotificationsConsumer(
            IDroidPushNotificationsParser pushNotificationsParser,
            IPushTokenStorageService pushTokenStorageService,
            IPushNotificationsHandler pushNotificationsHandler,
            IRemotePushNotificationsService remotePushNotificationsService,
            IDroidPushNotificationPresenter pushNotificationPresenter,
            IPushTokenSynchronizer pushTokenSynchronizer,
            ILogManager logManager)
        {
            _pushNotificationsParser = pushNotificationsParser;
            _pushTokenStorageService = pushTokenStorageService;
            _pushNotificationsHandler = pushNotificationsHandler;
            _remotePushNotificationsService = remotePushNotificationsService;
            _pushNotificationPresenter = pushNotificationPresenter;
            _pushTokenSynchronizer = pushTokenSynchronizer;
            _logger = logManager.GetLogger<DroidPushNotificationsConsumer>();

            _lifecycleObserver = new AppLifecycleObserver();
            ProcessLifecycleOwner.Get().Lifecycle.AddObserver(_lifecycleObserver);
        }

        /// <inheritdoc />
        public bool TryHandleNotification(RemoteMessage message)
        {
            if (!_pushNotificationsParser.TryParse(message, out var parsedNotification))
            {
                return false;
            }

            OnMessageReceivedInternal(parsedNotification, _lifecycleObserver.IsForegrounded);

            _pushNotificationPresenter.Present(parsedNotification, message.Data, _lifecycleObserver.IsForegrounded);

            return true;
        }

        /// <inheritdoc />
        public bool TryHandlePushNotificationIntent(Intent intent)
        {
            if (!_pushNotificationsParser.TryParse(intent.Extras, out var parsedNotification))
            {
                return false;
            }

            _pushNotificationsHandler.HandlePushNotificationTapped(parsedNotification);

            return true;
        }

        /// <inheritdoc />
        public void OnPushTokenRefreshed(string token)
        {
            var currentToken = _pushTokenStorageService.PushToken;
            var hasNewToken = !string.IsNullOrEmpty(currentToken) && currentToken != token;

            if (hasNewToken)
            {
                OnRegisteredForPushNotifications(token);
            }
        }

        /// <inheritdoc />
        public Task OnUnregisterFromPushNotifications()
        {
            return UnregisterFromRemotePushNotifications();
        }

        private void OnMessageReceivedInternal(PushNotificationModel parsedNotification, bool inForeground)
        {
            if (parsedNotification.IsSilent)
            {
                _pushNotificationsHandler.HandleSilentPushNotification(parsedNotification);
            }
            else
            {
                _pushNotificationsHandler.HandlePushNotificationReceived(parsedNotification, inForeground);
            }
        }

        private void OnRegisteredForPushNotifications(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                OnRegisterFailedInternal().FireAndForget(_logger);
            }
            else
            {
                OnRegisterSuccessInternal(token).FireAndForget(_logger);
            }
        }

        private async Task OnRegisterSuccessInternal(string token)
        {
            await _pushTokenSynchronizer.OnRegisterSuccessInternalAsync(token)
                .ConfigureAwait(false);

            _pushNotificationsHandler.OnPushRegistrationCompleted(
                _pushTokenStorageService.IsTokenRegisteredInSystem,
                _pushTokenStorageService.IsTokenSavedOnServer);
        }

        private async Task OnRegisterFailedInternal()
        {
            _pushTokenSynchronizer.OnRegisterFailedInternal();

            await UnregisterFromRemotePushNotifications();

            _pushNotificationsHandler.OnPushRegistrationCompleted(
                _pushTokenStorageService.IsTokenRegisteredInSystem,
                _pushTokenStorageService.IsTokenSavedOnServer);
        }

        private async Task UnregisterFromRemotePushNotifications()
        {
            if (!_pushTokenStorageService.IsTokenSavedOnServer)
            {
                return;
            }

            var tokenRemovedFromServer = await _remotePushNotificationsService
                .RemovePushNotificationsToken(_pushTokenStorageService.PushToken)
                .ConfigureAwait(false);
            if (tokenRemovedFromServer)
            {
                _pushTokenStorageService.IsTokenSavedOnServer = false;
            }
        }
    }
}
