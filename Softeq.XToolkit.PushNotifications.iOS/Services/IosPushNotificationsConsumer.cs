// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.PushNotifications.Abstract;
using Softeq.XToolkit.PushNotifications.iOS.Abstract;
using UIKit;
using UserNotifications;

namespace Softeq.XToolkit.PushNotifications.iOS.Services
{
    /// <summary>
    ///     Default implementation of <see cref="IIosPushNotificationsConsumer"/> interface for Android platform.
    /// </summary>
    public sealed class IosPushNotificationsConsumer : IIosPushNotificationsConsumer
    {
        private readonly IPushNotificationsHandler _pushNotificationsHandler;
        private readonly IIosPushNotificationsParser _pushNotificationsParser;
        private readonly INotificationCategoriesProvider _notificationCategoriesProvider;
        private readonly IPushTokenStorageService _pushTokenStorageService;
        private readonly IRemotePushNotificationsService _remotePushNotificationsService;
        private readonly ForegroundNotificationOptions _showForegroundNotificationsInSystemOptions;
        private readonly ILogger _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="IosPushNotificationsConsumer"/> class.
        /// </summary>
        /// <param name="pushNotificationsHandler">Handles received push notifications events.</param>
        /// <param name="pushNotificationsParser">Parses remote messages into common format.</param>
        /// <param name="notificationCategoriesProvider">Provides Notification Categories to be registered in system.</param>
        /// <param name="pushTokenStorageService">Stores push notification token and it's status.</param>
        /// <param name="remotePushNotificationsService">Propagates push notification token to remote server.</param>
        /// <param name="showForegroundNotificationsInSystemOptions">Defines how push notification should be presented.</param>
        /// <param name="logManager">Provides logging.</param>
        public IosPushNotificationsConsumer(
            IPushNotificationsHandler pushNotificationsHandler,
            IIosPushNotificationsParser pushNotificationsParser,
            INotificationCategoriesProvider notificationCategoriesProvider,
            IPushTokenStorageService pushTokenStorageService,
            IRemotePushNotificationsService remotePushNotificationsService,
            ForegroundNotificationOptions showForegroundNotificationsInSystemOptions,
            ILogManager logManager)
        {
            _pushNotificationsHandler = pushNotificationsHandler;
            _pushTokenStorageService = pushTokenStorageService;
            _remotePushNotificationsService = remotePushNotificationsService;
            _showForegroundNotificationsInSystemOptions = showForegroundNotificationsInSystemOptions;
            _notificationCategoriesProvider = notificationCategoriesProvider;
            _pushNotificationsParser = pushNotificationsParser;
            _logger = logManager.GetLogger<IosPushNotificationsConsumer>();
        }

        /// <inheritdoc />
        public UNAuthorizationOptions GetRequiredAuthorizationOptions()
        {
            return UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound | UNAuthorizationOptions.Badge;
        }

        /// <inheritdoc />
        public IEnumerable<UNNotificationCategory> GetCategories()
        {
            return _notificationCategoriesProvider.NotificationCategories;
        }

        /// <inheritdoc />
        public bool TryPresentNotification(
            UNUserNotificationCenter center,
            UNNotification notification,
            Action<UNNotificationPresentationOptions> completionHandler)
        {
            if (!_pushNotificationsParser.TryParse(notification.Request.Content.UserInfo, out var parsedNotification))
            {
                return false;
            }

            if (parsedNotification.IsSilent)
            {
                _pushNotificationsHandler.HandleSilentPushNotification(parsedNotification);
            }
            else
            {
                _pushNotificationsHandler.HandlePushNotificationReceived(parsedNotification, true);
            }

            switch (_showForegroundNotificationsInSystemOptions)
            {
                case ForegroundNotificationOptions.Show:
                    completionHandler.Invoke(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound);
                    break;
                case ForegroundNotificationOptions.ShowWithBadge:
                    completionHandler.Invoke(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound | UNNotificationPresentationOptions.Badge);
                    break;
                case ForegroundNotificationOptions.DoNotShow:
                default:
                    completionHandler.Invoke(UNNotificationPresentationOptions.None);
                    break;
            }

            return true;
        }

        /// <inheritdoc />
        public bool TryHandleNotificationResponse(
            UNUserNotificationCenter center,
            UNNotificationResponse response,
            Action completionHandler)
        {
            var actionIdentifier = response.ActionIdentifier;
            var userInfo = response.Notification.Request.Content.UserInfo;

            if (!_pushNotificationsParser.TryParse(userInfo, out var parsedNotification))
            {
                return false;
            }

            if (response.IsCustomAction)
            {
                var textInput = (response as UNTextInputNotificationResponse)?.UserText ?? string.Empty;
                _notificationCategoriesProvider.HandlePushNotificationCustomAction(parsedNotification, actionIdentifier, textInput);
            }
            else if (response.IsDefaultAction)
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
            else if (response.IsDismissAction)
            {
                _notificationCategoriesProvider.HandlePushNotificationCustomAction(parsedNotification, actionIdentifier, string.Empty);
            }

            completionHandler.Invoke();

            return true;
        }

        /// <inheritdoc />
        public bool TryHandleRemoteNotification(
            UIApplication application,
            NSDictionary userInfo,
            Action<UIBackgroundFetchResult> completionHandler)
        {
            // do nothing?
            return false;
        }

        /// <inheritdoc />
        public void OnPushNotificationAuthorizationResult(bool isGranted)
        {
            _pushNotificationsHandler.OnPushPermissionsRequestCompleted(isGranted);
        }

        /// <inheritdoc />
        public void OnRegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            var token = ParseDeviceToken(deviceToken);

            OnRegisteredForPushNotifications(token);
        }

        /// <inheritdoc />
        public void OnFailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            _logger.Warn($"Push Notifications failed to register: {error.Description}");
            OnRegisterFailedInternal().FireAndForget(_logger);
        }

        /// <inheritdoc />
        public Task OnUnregisterFromPushNotifications()
        {
            return UnregisterFromRemotePushNotifications();
        }

        private static string ParseDeviceToken(NSData deviceToken)
        {
            return deviceToken.AsString().Replace("-", string.Empty);
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
        }

        private async Task OnRegisterFailedInternal()
        {
            _pushTokenStorageService.IsTokenRegisteredInSystem = false;

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
