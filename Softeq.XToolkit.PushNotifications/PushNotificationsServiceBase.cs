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

        public PushNotificationsServiceBase(
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
        public abstract void RegisterForPushNotifications();
        protected abstract Task<bool> UnregisterFromPushTokenInSystem();

        public abstract void ClearAllNotifications();

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

        public void OnFailedToRegisterForPushNotifications(string errorMessage)
        {
            Logger.Warn($"Push Notifications failed to register: {errorMessage}");
            OnRegisterFailedInternal().SafeTaskWrapper(Logger);
        }

        private async Task OnRegisterSuccessInternal(string token)
        {
            PushTokenStorageService.IsTokenRegisteredInSystem = true;

            if (PushTokenStorageService.PushToken != token || !PushTokenStorageService.IsTokenSavedOnServer)
            {
                PushTokenStorageService.PushToken = token;
                await RegisterPushTokenOnServer();
            }

            PushNotificationsHandler.OnPushRegistrationCompleted(PushTokenStorageService.IsTokenRegisteredInSystem, PushTokenStorageService.IsTokenSavedOnServer);
        }

        private async Task OnRegisterFailedInternal()
        {
            PushTokenStorageService.IsTokenRegisteredInSystem = false;

            await UnregisterFromPushNotifications();

            PushNotificationsHandler.OnPushRegistrationCompleted(PushTokenStorageService.IsTokenRegisteredInSystem, PushTokenStorageService.IsTokenSavedOnServer);
        }

        public async Task<bool> UnregisterFromPushNotifications(bool unregisterInSystem = false)
        {
            // Unregister in system if needed
            if (PushTokenStorageService.IsTokenRegisteredInSystem && unregisterInSystem)
            {
                var unregistered = await UnregisterFromPushTokenInSystem();
                PushTokenStorageService.IsTokenRegisteredInSystem = !unregistered;
            }

            // Try to unregister on server
            await UnregisterPushTokenOnServer(PushTokenStorageService.PushToken);

            // Clear token if it is unregistered both in system and on server
            if (!PushTokenStorageService.IsTokenRegisteredInSystem && !PushTokenStorageService.IsTokenSavedOnServer)
            {
                PushTokenStorageService.PushToken = string.Empty;
            }

            return !PushTokenStorageService.IsTokenSavedOnServer && !(unregisterInSystem && PushTokenStorageService.IsTokenRegisteredInSystem);
        }

        public void OnMessageReceived(object pushNotification)
        {
            OnMessageReceivedInternal(pushNotification);
        }

        public void OnMessageTapped(object pushNotification)
        {
            var parsedNotification = ParsePushNotification(pushNotification);

            if (parsedNotification.IsSilent)
            {
                PushNotificationsHandler.HandleSilentPushNotification(parsedNotification);
                return;
            }

            PushNotificationsHandler.HandlePushNotificationTapped(parsedNotification);
        }

        protected virtual PushNotificationModel OnMessageReceivedInternal(object pushNotification)
        {
            var parsedNotification = ParsePushNotification(pushNotification);

            if (parsedNotification.IsSilent)
            {
                PushNotificationsHandler.HandleSilentPushNotification(parsedNotification);
            }
            else
            {
                PushNotificationsHandler.HandlePushNotificationReceived(parsedNotification);
            }

            return parsedNotification;
        }

        private PushNotificationModel ParsePushNotification(object pushNotification)
        {
            return PushNotificationParser.Parse(pushNotification);
        }

        private async Task RegisterPushTokenOnServer()
        {
            if (!string.IsNullOrEmpty(PushTokenStorageService.PushToken))
            {
                var savedOnServer = await RemotePushNotificationsService.SendPushNotificationsToken(PushTokenStorageService.PushToken);
                PushTokenStorageService.IsTokenSavedOnServer = savedOnServer;
            }
        }

        private async Task UnregisterPushTokenOnServer(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var removedFromServer = await RemotePushNotificationsService.RemovePushNotificationsToken(token);
                if (removedFromServer) // if we failed to remove it is left as it was (saved or not)
                {
                    PushTokenStorageService.IsTokenSavedOnServer = false;
                }
            }
        }
    }
}
