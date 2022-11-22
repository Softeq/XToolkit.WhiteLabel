// Developed for PAWS-HALO by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.PushNotifications.Abstract;

namespace Softeq.XToolkit.PushNotifications
{
    public class PushTokenSynchronizer : IPushTokenSynchronizer
    {
        private readonly IPushTokenStorageService _pushTokenStorageService;
        private readonly IRemotePushNotificationsService _remotePushNotificationsService;

        public PushTokenSynchronizer(
            IPushTokenStorageService pushTokenStorageService,
            IRemotePushNotificationsService remotePushNotificationsService)
        {
            _pushTokenStorageService = pushTokenStorageService;
            _remotePushNotificationsService = remotePushNotificationsService;
        }

        protected virtual TimeSpan TokenSendRetryDelay { get; } = TimeSpan.FromSeconds(10);

        protected virtual int MaxAttemptsCount { get; } = int.MaxValue;

        public async Task OnRegisterSuccessInternalAsync(string token)
        {
            _pushTokenStorageService.IsTokenRegisteredInSystem = true;

            if (_pushTokenStorageService.PushToken != token || !_pushTokenStorageService.IsTokenSavedOnServer)
            {
                _pushTokenStorageService.PushToken = token;

                if (!string.IsNullOrEmpty(token))
                {
                    await DoSendTokenToServer(token)
                        .ConfigureAwait(false);
                }
            }
        }

        public void OnRegisterFailedInternal()
        {
            _pushTokenStorageService.IsTokenRegisteredInSystem = false;
        }

        public Task ResendTokenToServerIfNeedAsync()
        {
            if (_pushTokenStorageService.IsTokenRegisteredInSystem
                && !_pushTokenStorageService.IsTokenSavedOnServer)
            {
                return DoSendTokenToServer(_pushTokenStorageService.PushToken);
            }

            return Task.CompletedTask;
        }

        private async Task DoSendTokenToServer(string token)
        {
            _pushTokenStorageService.IsTokenSavedOnServer = await _remotePushNotificationsService
                .SendPushNotificationsToken(token).ConfigureAwait(false);

            int attemptsCount = 1;

            while (!_pushTokenStorageService.IsTokenSavedOnServer && attemptsCount < MaxAttemptsCount)
            {
                await Task.Delay(TokenSendRetryDelay)
                    .ConfigureAwait(false);
                _pushTokenStorageService.IsTokenSavedOnServer = await _remotePushNotificationsService
                    .SendPushNotificationsToken(token).ConfigureAwait(false);
                attemptsCount++;
            }
        }
    }
}
