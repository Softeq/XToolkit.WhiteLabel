// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Softeq.XToolkit.Common.Logger;
using UIKit;
using UserNotifications;

namespace Softeq.XToolkit.PushNotifications.iOS
{
    public class IosPushNotificationsService : PushNotificationsServiceBase
    {
        /// <summary>
        /// Default Dismiss Action identifier
        /// </summary>
        public const string UNNotificationDismissActionIdentifier = "com.apple.UNNotificationDismissActionIdentifier";

        private readonly INotificationsPermissionsService _permissionsService;
        private readonly INotificationCategoriesProvider _notificationCategoriesProvider;

        private bool _isInitialized;

        public IosPushNotificationsService(
            IRemotePushNotificationsService remotePushNotificationsService,
            IPushTokenStorageService pushTokenStorageService,
            IPushNotificationsHandler pushNotificationsHandler,
            IPushNotificationParser pushNotificationParser,
            INotificationsPermissionsService permissionsService,
            INotificationCategoriesProvider notificationCategoriesProvider,
            ILogManager logManager)
            : base(remotePushNotificationsService, pushTokenStorageService, pushNotificationsHandler, pushNotificationParser,
                logManager)
        {
            _permissionsService = permissionsService;
            _notificationCategoriesProvider = notificationCategoriesProvider;
        }

        public override void Initialize(ForegroundNotificationOptions showForegroundNotificationsInSystemOptions)
        {
            if (_isInitialized)
            {
                throw new ArgumentException($"{nameof(IosPushNotificationsService)}: Already Initialized");
            }

            _isInitialized = true;

            UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate(this, showForegroundNotificationsInSystemOptions);
            UNUserNotificationCenter.Current.SetNotificationCategories(new NSSet<UNNotificationCategory>(_notificationCategoriesProvider.NotificationCategories.ToArray()));
        }

        public override void RegisterForPushNotifications()
        {
            UIApplication.SharedApplication.InvokeOnMainThread(async () =>
            {
                var permissionGranted = await _permissionsService.RequestNotificationsPermissions();

                // TODO: we should register token in any case to handle the possibility of user changing permission in Settings
                // (the system itself will disregard notifications if permissions are not granted)
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
            base.OnRegisteredForPushNotifications(SimplifyToken(token));
        }

        protected override void SetBadgeNumberInternal(int badgeNumber)
        {
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = badgeNumber;
        }

        protected override void OnMessageCustomActionInvokedInternal(PushNotificationModel parsedNotification, string actionId, string textInput)
        {
            _notificationCategoriesProvider.HandlePushNotificationCustomAction(parsedNotification, actionId, textInput);
        }

        protected override Task<bool> UnregisterFromPushTokenInSystem()
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.UnregisterForRemoteNotifications();
            });
            return Task.FromResult(true);
        }

        private string SimplifyToken(string token)
        {
            return string.IsNullOrWhiteSpace(token) ? token : token.Replace("-", string.Empty);
        }
    }
}
