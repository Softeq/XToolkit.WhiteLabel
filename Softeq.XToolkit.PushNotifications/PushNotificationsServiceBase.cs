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

        public PushNotificationsServiceBase(IRemotePushNotificationsService remotePushNotificationsService,
            IPushTokenStorageService pushTokenStorageService, IPushNotificationsHandler pushNotificationsHandler,
            IPushNotificationParser pushNotificationParser, ILogger logger)
        {
            RemotePushNotificationsService = remotePushNotificationsService;
            PushTokenStorageService = pushTokenStorageService;
            PushNotificationsHandler = pushNotificationsHandler;
            PushNotificationParser = pushNotificationParser;
            Logger = logger;
        }

        public abstract void Initialize(bool showForegroundNotificationsInSystem);
        public abstract void RegisterForPushNotifications();
        protected abstract void UnregisterFromPushTokenInSystem();

        protected abstract string SimplifyToken(string token); // Required for iOS
        protected abstract void ShowNotification(object pushNotification, PushNotificationModel parsedPushNotification); // Required for Android

        public abstract void ClearAllNotifications();

        public void OnRegisteredForPushNotificaions(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                OnFailedToRegisterForPushNotifications("OnRegistered with empty token");
            }
            else
            {
                token = SimplifyToken(token);
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
            var parsedNotification = ParsePushNotification(pushNotification);

            if (parsedNotification.IsSilent)
            {
                PushNotificationsHandler.HandleSilentPushNotification(parsedNotification);
                return;
            }

            ShowNotification(pushNotification, parsedNotification);
            PushNotificationsHandler.HandlePushNotificationReceived(parsedNotification);
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
