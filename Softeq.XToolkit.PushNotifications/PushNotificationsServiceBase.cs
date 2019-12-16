// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Logger;

namespace Softeq.XToolkit.PushNotifications
{
    public abstract class PushNotificationsServiceBase : IPushNotificationsService
    {
        protected readonly ILogger Logger;
        protected readonly IPushNotificationParser PushNotificationParser;
        protected readonly IPushNotificationsHandler PushNotificationsHandler;
        protected readonly IPushTokenStorageService PushTokenStorageService;
        protected readonly IRemotePushNotificationsService RemotePushNotificationsService;

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

        public abstract void Initialize(ForegroundNotificationOptions showForegroundNotificationsInSystemOptions);

        public abstract void ClearAllNotifications();

        public abstract void RegisterForPushNotifications();

        public async Task<PushNotificationsUnregisterResult> UnregisterFromPushNotifications(
            PushNotificationsUnregisterOptions options = PushNotificationsUnregisterOptions.OnServerOnly)
        {
            // Unregister in system if needed
            if (options.ShouldUnregisterInSystem() && PushTokenStorageService.IsTokenRegisteredInSystem)
            {
                var tokenRemovedFromSystem = await UnregisterFromPushTokenInSystem();
                if (!tokenRemovedFromSystem)
                {
                    return PushNotificationsUnregisterResult.Failed;
                }

                PushTokenStorageService.IsTokenRegisteredInSystem = false;
            }

            var token = PushTokenStorageService.PushToken;
            if (string.IsNullOrEmpty(token))
            {
                return PushNotificationsUnregisterResult.Success;
            }

            // Unregister on server if needed
            if (options.ShouldUnregisterOnServer() && PushTokenStorageService.IsTokenSavedOnServer)
            {
                var tokenRemovedFromServer = await RemotePushNotificationsService
                    .RemovePushNotificationsToken(token).ConfigureAwait(false);
                if (!tokenRemovedFromServer)
                {
                    return PushNotificationsUnregisterResult.ServerFailed;
                }

                PushTokenStorageService.IsTokenSavedOnServer = false;
            }

            // Clear token if it is unregistered both in the system (optional) and on server
            PushTokenStorageService.PushToken = string.Empty;

            return PushNotificationsUnregisterResult.Success;
        }

        [Obsolete("Use UnregisterFromPushNotifications with PushNotificationsUnregisterOptions param instead.")]
        public async Task<PushNotificationsUnregisterResult> UnregisterFromPushNotifications(bool unregisterInSystem = false)
        {
            return await UnregisterFromPushNotifications(unregisterInSystem
                ? PushNotificationsUnregisterOptions.InSystemAndOnServer
                : PushNotificationsUnregisterOptions.OnServerOnly);
        }

        public virtual void OnRegisteredForPushNotifications(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                OnFailedToRegisterForPushNotifications("OnRegistered with empty token");
            }
            else
            {
                OnRegisterSuccessInternal(token).FireAndForget(Logger);
            }
        }

        public virtual void OnFailedToRegisterForPushNotifications(string errorMessage)
        {
            Logger.Warn($"Push Notifications failed to register: {errorMessage}");
            OnRegisterFailedInternal().FireAndForget(Logger);
        }

        public void OnMessageReceived(object pushNotification, bool inForeground)
        {
            if (TryParsePushNotification(pushNotification, out var parsedNotification))
            {
                OnMessageReceivedInternal(pushNotification, parsedNotification, inForeground);
            }
        }

        public void OnMessageTapped(object pushNotification)
        {
            if (TryParsePushNotification(pushNotification, out var parsedNotification))
            {
                if (parsedNotification.IsSilent)
                {
                    PushNotificationsHandler.HandleSilentPushNotification(parsedNotification);
                }
                else
                {
                    PushNotificationsHandler.HandlePushNotificationTapped(parsedNotification);
                }
            }
        }

        public void OnMessageCustomActionInvoked(object pushNotification, string actionId, string textInput)
        {
            if (TryParsePushNotification(pushNotification, out var parsedNotification))
            {
                OnMessageCustomActionInvokedInternal(parsedNotification, actionId, textInput);
            }
        }

        public void SetBadgeNumber(int badgeNumber)
        {
            SetBadgeNumberInternal(badgeNumber <= 0 ? 0 : badgeNumber);
        }

        protected abstract void SetBadgeNumberInternal(int badgeNumber);

        protected abstract Task<bool> UnregisterFromPushTokenInSystem();

        protected abstract void OnMessageCustomActionInvokedInternal(PushNotificationModel parsedNotification, string actionId, string textInput);

        // ReSharper disable once UnusedMethodReturnValue.Global - used on Android
        protected virtual void OnMessageReceivedInternal(object pushNotification, PushNotificationModel parsedNotification, bool inForeground)
        {
            if (parsedNotification.IsSilent)
            {
                PushNotificationsHandler.HandleSilentPushNotification(parsedNotification);
            }
            else
            {
                PushNotificationsHandler.HandlePushNotificationReceived(parsedNotification, inForeground);
            }
        }

        private bool TryParsePushNotification(object pushNotification, out PushNotificationModel result)
        {
            try
            {
                result = PushNotificationParser.Parse(pushNotification);
            }
            catch (Exception ex)
            {
                PushNotificationsHandler.HandleInvalidPushNotification(ex, pushNotification);
                result = null;
            }

            return result != null;
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
