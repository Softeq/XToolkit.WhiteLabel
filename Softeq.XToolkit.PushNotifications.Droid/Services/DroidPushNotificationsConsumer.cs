// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Android.App;
using AndroidX.Lifecycle;
using Firebase.Messaging;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.PushNotifications.Droid.Abstract;

namespace Softeq.XToolkit.PushNotifications.Droid.Services
{
    public class DroidPushNotificationsConsumer : IPushNotificationsConsumer
    {
        private readonly IPushTokenStorageService _pushTokenStorageService;
        private readonly IPushNotificationsParser _pushNotificationsParser;
        private readonly IPushNotificationsHandler _pushNotificationsHandler;
        private readonly IRemotePushNotificationsService _remotePushNotificationsService;
        private readonly ForegroundNotificationOptions _showForegroundNotificationsInSystemOptions;
        private readonly ILogger _logger;

        private readonly AppLifecycleObserver _lifecycleObserver;

        public DroidPushNotificationsConsumer(
            INotificationsSettingsProvider notificationsSettings,
            IPushNotificationsParser pushNotificationsParser,
            IPushTokenStorageService pushTokenStorageService,
            IPushNotificationsHandler pushNotificationsHandler,
            IRemotePushNotificationsService remotePushNotificationsService,
            ForegroundNotificationOptions showForegroundNotificationsInSystemOptions,
            ILogManager logManager)
        {
            _pushTokenStorageService = pushTokenStorageService;
            _pushNotificationsHandler = pushNotificationsHandler;
            _remotePushNotificationsService = remotePushNotificationsService;
            _showForegroundNotificationsInSystemOptions = showForegroundNotificationsInSystemOptions;
            _pushNotificationsParser = pushNotificationsParser;
            _logger = logManager.GetLogger<DroidPushNotificationsConsumer>();

            NotificationsHelper.Init(notificationsSettings);

            _lifecycleObserver = new AppLifecycleObserver();
            ProcessLifecycleOwner.Get().Lifecycle.AddObserver(_lifecycleObserver);
        }

        public bool TryHandleNotification(RemoteMessage message)
        {
            if (!TryParsePushNotification(message, out var parsedNotification))
            {
                return false;
            }

            OnMessageReceivedInternal(parsedNotification, _lifecycleObserver.IsForegrounded);

            if (!parsedNotification.IsSilent && (_showForegroundNotificationsInSystemOptions.ShouldShow() || !_lifecycleObserver.IsForegrounded))
            {
                NotificationsHelper.CreateNotification(Application.Context, parsedNotification, message.Data);
            }

            return true;
        }

        private bool TryParsePushNotification(RemoteMessage message, out PushNotificationModel result)
        {
            try
            {
                result = _pushNotificationsParser.Parse(message);
                return true;
            }
            catch (Exception ex)
            {
                _pushNotificationsHandler.HandleInvalidPushNotification(ex, message);
            }

            result = new PushNotificationModel();
            return false;
        }

        protected virtual void OnMessageReceivedInternal(PushNotificationModel parsedNotification, bool inForeground)
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

        public void OnPushTokenRefreshed(string token)
        {
            var currentToken = _pushTokenStorageService.PushToken;
            var hasNewToken = !string.IsNullOrEmpty(currentToken) && currentToken != token;

            if (hasNewToken)
            {
                OnRegisteredForPushNotifications(token);
            }
        }

        public Task OnUnregisterFromPushNotifications()
        {
            return UnregisterFromPushNotifications();
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
            _pushTokenStorageService.IsTokenRegisteredInSystem = true;

            if (_pushTokenStorageService.PushToken != token || !_pushTokenStorageService.IsTokenSavedOnServer)
            {
                _pushTokenStorageService.PushToken = token;

                if (!string.IsNullOrEmpty(token))
                {
                    _pushTokenStorageService.IsTokenSavedOnServer = await _remotePushNotificationsService
                        .SendPushNotificationsToken(token).ConfigureAwait(false);
                }
            }

            _pushNotificationsHandler.OnPushRegistrationCompleted(
                _pushTokenStorageService.IsTokenRegisteredInSystem,
                _pushTokenStorageService.IsTokenSavedOnServer);

            // _registrationCompletionSource?.TrySetResult(new PushNotificationRegistrationResult(
            //     _pushTokenStorageService.IsTokenRegisteredInSystem, _pushTokenStorageService.IsTokenSavedOnServer));
        }

        private async Task OnRegisterFailedInternal()
        {
            _pushTokenStorageService.IsTokenRegisteredInSystem = false;

            await UnregisterFromPushNotifications();

            _pushNotificationsHandler.OnPushRegistrationCompleted(
                _pushTokenStorageService.IsTokenRegisteredInSystem,
                _pushTokenStorageService.IsTokenSavedOnServer);

            // _registrationCompletionSource?.TrySetResult(new PushNotificationRegistrationResult(
            //     _pushTokenStorageService.IsTokenRegisteredInSystem, _pushTokenStorageService.IsTokenSavedOnServer));
        }

        private async Task UnregisterFromPushNotifications()
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
