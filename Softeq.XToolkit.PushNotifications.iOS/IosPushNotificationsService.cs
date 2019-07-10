// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Interfaces;
using UIKit;
using UserNotifications;

namespace Softeq.XToolkit.PushNotifications.iOS
{
    public class IosPushNotificationsService : PushNotificationsServiceBase
    {
        private readonly INotificationsPermissionsService _permissionsService;

        private bool _isInitialized;

        // badges

        public IosPushNotificationsService(
            IRemotePushNotificationsService remotePushNotificationsService,
            IPushTokenStorageService pushTokenStorageService,
            IPushNotificationsHandler pushNotificationsHandler,
            IPushNotificationParser pushNotificationParser,
            INotificationsPermissionsService permissionsService,
            ILogManager logManager)
            : base(remotePushNotificationsService, pushTokenStorageService, pushNotificationsHandler, pushNotificationParser, logManager)
        {
            _permissionsService = permissionsService;
        }

        public override void Initialize(bool showForegroundNotificationsInSystem)
        {
            if (_isInitialized)
            {
                throw new ArgumentException($"{nameof(IosPushNotificationsService)}: Already Initialized");
            }

            _isInitialized = true;
            UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate(this, showForegroundNotificationsInSystem);
        }

        public override void RegisterForPushNotifications()
        {
            UIApplication.SharedApplication.InvokeOnMainThread(async () =>
            {
                var permissionGranted = await _permissionsService.RequestNotificationsPermissions();

                if (permissionGranted)
                {
                    UIApplication.SharedApplication.RegisterForRemoteNotifications();
                }
                else
                {
                    OnFailedToRegisterForPushNotifications("Permission denied");
                }
            });
        }

        public override void ClearAllNotifications()
        {
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
            UNUserNotificationCenter.Current.RemoveAllDeliveredNotifications();
            UNUserNotificationCenter.Current.RemoveAllPendingNotificationRequests();
        }

        public override void OnRegisteredForPushNotifications(string token)
        {
            token = SimplifyToken(token);
            base.OnRegisteredForPushNotifications(token);
        }

        protected override Task<bool> UnregisterFromPushTokenInSystem()
        {
            UIApplication.SharedApplication.UnregisterForRemoteNotifications();
            return Task.FromResult(true);
        }

        private string SimplifyToken(string token)
        {
            return string.IsNullOrWhiteSpace(token) ? token : token.Trim('<').Trim('>').Replace(" ", string.Empty);
        }
    }

    public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        private readonly IPushNotificationsReceiver _pushNotificationsReceiver;
        private readonly bool _showForegroundNotificationsInSystem;

        public UserNotificationCenterDelegate(IPushNotificationsReceiver pushNotificationsReceiver, bool showForegroundNotificationsInSystem)
        {
            _pushNotificationsReceiver = pushNotificationsReceiver;
            _showForegroundNotificationsInSystem = showForegroundNotificationsInSystem;
        }

        // Handle Foreground Notifications
        public override void WillPresentNotification(UNUserNotificationCenter center,
                                                     UNNotification notification,
                                                     Action<UNNotificationPresentationOptions> completionHandler)
        {
            _pushNotificationsReceiver.OnMessageReceived(notification.Request.Content.UserInfo);
            if (_showForegroundNotificationsInSystem)
            {
                completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound);
            }
            else
            {
                completionHandler(UNNotificationPresentationOptions.None);
            }
        }

        // Called when notification is tapped
        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center,
                                                            UNNotificationResponse response,
                                                            Action completionHandler)
        {
            _pushNotificationsReceiver.OnMessageTapped(response.Notification.Request.Content.UserInfo);
            completionHandler.Invoke();
        }
    }
}
