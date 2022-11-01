// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.PushNotifications.iOS.Abstract;
using UIKit;
using UserNotifications;

namespace Softeq.XToolkit.PushNotifications.iOS.Services
{
    public class IosPushNotificationsConsumer : IPushNotificationsConsumer
    {
        private readonly IPushNotificationsHandler _pushNotificationsHandler;
        private readonly IPushNotificationsParser _pushNotificationsParser;
        private readonly INotificationCategoriesProvider _notificationCategoriesProvider;
        private readonly IPushTokenStorageService _pushTokenStorageService;
        private readonly IRemotePushNotificationsService _remotePushNotificationsService;
        private readonly ILogger _logger;
        private readonly ForegroundNotificationOptions _showForegroundNotificationsInSystemOptions;

        public IosPushNotificationsConsumer(
            IPushNotificationsHandler pushNotificationsHandler,
            IPushNotificationsParser pushNotificationsParser,
            INotificationCategoriesProvider notificationCategoriesProvider,
            IPushTokenStorageService pushTokenStorageService,
            IRemotePushNotificationsService remotePushNotificationsService,
            ILogManager logManager,
            ForegroundNotificationOptions showForegroundNotificationsInSystemOptions)
        {
            _pushNotificationsHandler = pushNotificationsHandler;
            _pushTokenStorageService = pushTokenStorageService;
            _remotePushNotificationsService = remotePushNotificationsService;
            _logger = logManager.GetLogger<IosPushNotificationsConsumer>();
            _showForegroundNotificationsInSystemOptions = showForegroundNotificationsInSystemOptions;
            _notificationCategoriesProvider = notificationCategoriesProvider;
            _pushNotificationsParser = pushNotificationsParser;
        }

        public IEnumerable<UNNotificationCategory> GetCategories()
        {
            return _notificationCategoriesProvider.NotificationCategories;
        }

        public void WillPresentNotification(
            UNUserNotificationCenter center,
            UNNotification notification,
            Action<UNNotificationPresentationOptions> completionHandler)
        {
            if (TryParsePushNotification(notification.Request.Content.UserInfo, out var parsedNotification))
            {
                if (parsedNotification.IsSilent)
                {
                    _pushNotificationsHandler.HandleSilentPushNotification(parsedNotification);
                }
                else
                {
                    _pushNotificationsHandler.HandlePushNotificationReceived(parsedNotification, true);
                }
            }

            switch (_showForegroundNotificationsInSystemOptions)
            {
                case ForegroundNotificationOptions.Show:
                    completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound);
                    break;
                case ForegroundNotificationOptions.ShowWithBadge:
                    completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound | UNNotificationPresentationOptions.Badge);
                    break;
                case ForegroundNotificationOptions.DoNotShow:
                    completionHandler(UNNotificationPresentationOptions.None);
                    break;
            }
        }

        private bool TryParsePushNotification(NSDictionary userInfo, out PushNotificationModel result)
        {
            try
            {
                result = _pushNotificationsParser.Parse(userInfo);
                return true;
            }
            catch (Exception ex)
            {
                _pushNotificationsHandler.HandleInvalidPushNotification(ex, userInfo);
            }

            result = new PushNotificationModel();
            return false;
        }

        public void DidReceiveNotificationResponse(
            UNUserNotificationCenter center,
            UNNotificationResponse response,
            Action completionHandler)
        {
            var actionIdentifier = response.ActionIdentifier;
            var userInfo = response.Notification.Request.Content.UserInfo;

            if (response.IsCustomAction)
            {
                var textInput = (response as UNTextInputNotificationResponse)?.UserText ?? string.Empty;
                if (TryParsePushNotification(userInfo, out var parsedNotification))
                {
                    _notificationCategoriesProvider.HandlePushNotificationCustomAction(parsedNotification, actionIdentifier, textInput);
                }
            }
            else if (response.IsDefaultAction)
            {
                if (TryParsePushNotification(userInfo, out var parsedNotification))
                {
                    if (parsedNotification.IsSilent)
                    {
                        _pushNotificationsHandler.HandleSilentPushNotification(parsedNotification);
                    }
                    else
                    {
                        _pushNotificationsHandler.HandlePushNotificationTapped(parsedNotification);
                    }
                }
            }
            else if (response.IsDismissAction)
            {
                if (TryParsePushNotification(userInfo, out var parsedNotification))
                {
                    _notificationCategoriesProvider.HandlePushNotificationCustomAction(parsedNotification, actionIdentifier, string.Empty);
                }
            }

            completionHandler.Invoke();
        }

        public void OnPushNotificationAuthorizationResult(bool isGranted)
        {
            _pushNotificationsHandler.OnPushPermissionsRequestCompleted(isGranted);
        }

        public void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            var token = ParseDeviceToken(deviceToken);

            OnRegisteredForPushNotifications(token);
        }

        public void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            _logger.Warn($"Push Notifications failed to register: {error.Description}");
            OnRegisterFailedInternal().FireAndForget(_logger);
        }

        public void DidReceiveRemoteNotification(
            UIApplication application,
            NSDictionary userInfo,
            Action<UIBackgroundFetchResult> completionHandler)
        {
            // do nothing?
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

        private static string ParseDeviceToken(NSData deviceToken)
        {
            return deviceToken.ToString().Replace("-", string.Empty);
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
