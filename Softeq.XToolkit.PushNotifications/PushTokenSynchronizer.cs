using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.PushNotifications.Abstract;

namespace Softeq.XToolkit.PushNotifications
{
    public class PushTokenSynchronizer : IPushTokenSynchronizer
    {
        private readonly string _isTokenRegisteredInSystemKey = $"{nameof(IPushTokenSynchronizer)}_token_registered_in_system_key";
        private readonly string _isTokenSavedOnServerKey = $"{nameof(IPushTokenSynchronizer)}_token_saved_on_server_key";

        private readonly IPushTokenStorageService _pushTokenStorageService;
        private readonly IRemotePushNotificationsService _remotePushNotificationsService;
        private readonly IInternalSettings _internalSettings;
        private readonly IPushNotificationsHandler _pushNotificationsHandler;
        private readonly ILogger _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PushTokenSynchronizer"/> class.
        /// </summary>
        /// <param name="pushNotificationsHandler">Handles received push notifications events.</param>
        /// <param name="internalSettings">Parses remote messages into common format.</param>
        /// <param name="pushTokenStorageService">Stores push notification token and it's status.</param>
        /// <param name="remotePushNotificationsService">Propagates push notification token to remote server.</param>
        /// <param name="logManager">Provides logging.</param>
        public PushTokenSynchronizer(
            IPushNotificationsHandler pushNotificationsHandler,
            IInternalSettings internalSettings,
            IPushTokenStorageService pushTokenStorageService,
            IRemotePushNotificationsService remotePushNotificationsService,
            ILogManager logManager)
        {
            _pushNotificationsHandler = pushNotificationsHandler;
            _internalSettings = internalSettings;
            _pushTokenStorageService = pushTokenStorageService;
            _remotePushNotificationsService = remotePushNotificationsService;

            _logger = logManager.GetLogger<PushTokenSynchronizer>();
        }

        protected virtual TimeSpan TokenSendRetryDelay { get; } = TimeSpan.FromSeconds(10);

        protected virtual int MaxAttemptsCount { get; } = int.MaxValue;

        public bool IsTokenSavedOnServer
        {
            get => _internalSettings.GetValueOrDefault(_isTokenSavedOnServerKey, default(bool));
            set => _internalSettings.AddOrUpdateValue(_isTokenSavedOnServerKey, value);
        }

        private bool IsTokenRegisteredInSystem
        {
            get => _internalSettings.GetValueOrDefault(_isTokenRegisteredInSystemKey, default(bool));
            set => _internalSettings.AddOrUpdateValue(_isTokenRegisteredInSystemKey, value);
        }

        /// <inheritdoc />
        public Task ResendTokenToServerIfNeedAsync()
        {
            if (IsTokenRegisteredInSystem && !IsTokenSavedOnServer)
            {
                return DoSendTokenToServer(_pushTokenStorageService.PushToken);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task UnregisterFromRemotePushNotificationsAsync()
        {
            if (!IsTokenSavedOnServer)
            {
                return;
            }

            var tokenRemovedFromServer = await _remotePushNotificationsService
                .RemovePushNotificationsToken(_pushTokenStorageService.PushToken)
                .ConfigureAwait(false);
            if (tokenRemovedFromServer)
            {
                IsTokenSavedOnServer = false;
            }
        }

        /// <inheritdoc />
        public void OnTokenChanged(string token)
        {
            if (_pushTokenStorageService.PushToken == token)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                OnRegisterFailedInternalAsync().FireAndForget(_logger);
            }
            else
            {
                OnRegisterSuccessInternalAsync(token).FireAndForget(_logger);
            }
        }

        /// <inheritdoc />
        public async Task OnRegisterFailedInternalAsync()
        {
            IsTokenRegisteredInSystem = false;

            await UnregisterFromRemotePushNotificationsAsync()
                .ConfigureAwait(false);

            _pushNotificationsHandler.OnPushRegistrationCompleted(
                false,
                IsTokenSavedOnServer);
        }

        private async Task OnRegisterSuccessInternalAsync(string token)
        {
            IsTokenRegisteredInSystem = true;

            if (!IsTokenSavedOnServer)
            {
                _pushTokenStorageService.PushToken = token;

                await DoSendTokenToServer(token)
                    .ConfigureAwait(false);
            }

            _pushNotificationsHandler.OnPushRegistrationCompleted(
                true,
                IsTokenSavedOnServer);
        }

        private async Task DoSendTokenToServer(string token)
        {
            IsTokenSavedOnServer = await _remotePushNotificationsService
                .SendPushNotificationsToken(token).ConfigureAwait(false);

            int attemptsCount = 1;

            while (!IsTokenSavedOnServer && attemptsCount < MaxAttemptsCount)
            {
                await Task.Delay(TokenSendRetryDelay)
                    .ConfigureAwait(false);
                IsTokenSavedOnServer = await _remotePushNotificationsService
                    .SendPushNotificationsToken(token).ConfigureAwait(false);
                attemptsCount++;
            }
        }
    }
}
