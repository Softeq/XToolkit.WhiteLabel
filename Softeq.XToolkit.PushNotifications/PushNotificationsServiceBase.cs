// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.PushNotifications
{
    public abstract class PushNotificationsServiceBase : IPushNotificationsService
    {
        protected readonly IRemotePushNotificationsService RemotePushNotificationsService;
        protected readonly IPushTokenStorageService PushTokenStorageService;
        protected readonly IPushNotificationsHandler PushNotificationsHandler;
        protected readonly IPushNotificationParser PushNotificationParser;
        protected readonly ILogger Logger;

        protected PushNotificationsServiceBase(
            IRemotePushNotificationsService remotePushNotificationsService,
            IPushTokenStorageService pushTokenStorageService,
            IPushNotificationsHandler pushNotificationsHandler,
            IPushNotificationParser pushNotificationParser,
            ILogManager logManager)
        {
            RemotePushNotificationsService = remotePushNotificationsService;
            PushTokenStorageService = pushTokenStorageService;
            PushNotificationsHandler = pushNotificationsHandler;
            PushNotificationParser = pushNotificationParser;
            Logger = logManager.GetLogger<PushNotificationsServiceBase>();
        }

        public abstract void Initialize(bool showForegroundNotificationsInSystem);

        public abstract void ClearAllNotifications();

        public abstract void RegisterForPushNotifications();

        public async Task<PushNotificationsUnregisterResult> UnregisterFromPushNotifications(bool unregisterInSystem = false)
        {
            // Unregister in system if needed
            if (PushTokenStorageService.IsTokenRegisteredInSystem && unregisterInSystem)
            {
                var unregistered = await UnregisterFromPushTokenInSystem();
                PushTokenStorageService.IsTokenRegisteredInSystem = !unregistered;
            }

            var token = PushTokenStorageService.PushToken;
            if (string.IsNullOrEmpty(token))
            {
                return PushNotificationsUnregisterResult.Success;
            }

            // Unregister on server
            var tokenRemovedFromServer = await RemotePushNotificationsService
                .RemovePushNotificationsToken(token).ConfigureAwait(false);
            if (tokenRemovedFromServer)
            {
                PushTokenStorageService.IsTokenSavedOnServer = false;
            }

            // Check on errors
            if (PushTokenStorageService.IsTokenRegisteredInSystem ||
                PushTokenStorageService.IsTokenSavedOnServer)
            {
                return tokenRemovedFromServer
                    ? PushNotificationsUnregisterResult.Failed
                    : PushNotificationsUnregisterResult.ServerFailed;
            }

            // Clear token if it is unregistered both in system and on server
            PushTokenStorageService.PushToken = string.Empty;

            return PushNotificationsUnregisterResult.Success;
        }

        public virtual void OnRegisteredForPushNotifications(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                OnFailedToRegisterForPushNotifications("OnRegistered with empty token");
            }
            else
            {
                OnRegisterSuccessInternal(token).SafeTaskWrapper(Logger);
            }
        }

        public virtual void OnFailedToRegisterForPushNotifications(string errorMessage)
        {
            Logger.Warn($"Push Notifications failed to register: {errorMessage}");
            OnRegisterFailedInternal().SafeTaskWrapper(Logger);
        }

        public void OnMessageReceived(object pushNotification, bool inForeground)
        {
            OnMessageReceivedInternal(pushNotification, inForeground);
        }

        public void OnMessageTapped(object pushNotification)
        {
            var parsedNotification = PushNotificationParser.Parse(pushNotification);

            if (parsedNotification.IsSilent)
            {
                PushNotificationsHandler.HandleSilentPushNotification(parsedNotification);
                return;
            }

            PushNotificationsHandler.HandlePushNotificationTapped(parsedNotification);
        }

        protected abstract Task<bool> UnregisterFromPushTokenInSystem();

        // ReSharper disable once UnusedMethodReturnValue.Global - used on Android
        protected virtual PushNotificationModel OnMessageReceivedInternal(object pushNotification, bool inForeground)
        {
            var parsedNotification = PushNotificationParser.Parse(pushNotification);

            if (parsedNotification.IsSilent)
            {
                PushNotificationsHandler.HandleSilentPushNotification(parsedNotification);
            }
            else
            {
                PushNotificationsHandler.HandlePushNotificationReceived(parsedNotification, inForeground);
            }

            return parsedNotification;
        }

        private async Task OnRegisterSuccessInternal(string token)
        {
            PushTokenStorageService.IsTokenRegisteredInSystem = true;

            if (PushTokenStorageService.PushToken != token || !PushTokenStorageService.IsTokenSavedOnServer)
            {
                PushTokenStorageService.PushToken = token;

                if (!string.IsNullOrEmpty(token))
                {
                    PushTokenStorageService.IsTokenSavedOnServer = await RemotePushNotificationsService
                        .SendPushNotificationsToken(token).ConfigureAwait(false);
                }
            }

            PushNotificationsHandler.OnPushRegistrationCompleted(
                PushTokenStorageService.IsTokenRegisteredInSystem,
                PushTokenStorageService.IsTokenSavedOnServer);
        }

        private async Task OnRegisterFailedInternal()
        {
            PushTokenStorageService.IsTokenRegisteredInSystem = false;

            await UnregisterFromPushNotifications();

            PushNotificationsHandler.OnPushRegistrationCompleted(
                PushTokenStorageService.IsTokenRegisteredInSystem,
                PushTokenStorageService.IsTokenSavedOnServer);
        }
    }
}
