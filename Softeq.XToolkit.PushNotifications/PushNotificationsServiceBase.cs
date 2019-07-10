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

        protected string PushToken => PushTokenStorageService?.PushToken;

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
        protected abstract void UnregisterFromPushTokenInSystem();

        public abstract void ClearAllNotifications();

        public virtual void OnRegisteredForPushNotifications(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                OnFailedToRegisterForPushNotifications("OnRegistered with empty token");
            }
            else
            {
                PushTokenStorageService.PushToken = token;

                RegisterPushTokenOnServer().SafeTaskWrapper(Logger);
            }
        }

        public void OnFailedToRegisterForPushNotifications(string errorMessage)
        {
            Logger.Warn($"Push Notifications failed to register: {errorMessage}");
            UnregisterFromPushNotifications().SafeTaskWrapper(Logger);
        }

        public async Task UnregisterFromPushNotifications(bool unregisterInSystem = false)
        {
            if (unregisterInSystem)
            {
                UnregisterFromPushTokenInSystem();
            }
            var token = PushTokenStorageService.PushToken;
            PushTokenStorageService.PushToken = string.Empty;
            await UnregisterPushTokenOnServer(token);
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
            if (!string.IsNullOrEmpty(PushToken))
            {
                await RemotePushNotificationsService.SendPushNotificationsToken(PushToken);
            }
        }

        private async Task UnregisterPushTokenOnServer(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                await RemotePushNotificationsService.RemovePushNotificationsToken(token);
            }
        }
    }
}
