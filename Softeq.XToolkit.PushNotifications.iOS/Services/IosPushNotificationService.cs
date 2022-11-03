// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.PushNotifications.Abstract;
using Softeq.XToolkit.PushNotifications.iOS.Abstract;
using UIKit;
using UserNotifications;

namespace Softeq.XToolkit.PushNotifications.iOS.Services
{
    /// <summary>
    ///     Default implementation of <see cref="IPushNotificationsService"/> interface for iOS platform.
    /// </summary>
    public sealed class IosPushNotificationService : IPushNotificationsService, IPushNotificationAppDelegate
    {
        private readonly IPushNotificationsConsumer _pushNotificationsConsumer;
        private readonly ILogger _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="IosPushNotificationService"/> class.
        /// </summary>
        /// <param name="pushNotificationsConsumer">Consumer of the push notification related callbacks.</param>
        /// <param name="logManager">Provides logging.</param>
        public IosPushNotificationService(
            IPushNotificationsConsumer pushNotificationsConsumer,
            ILogManager logManager)
        {
            _pushNotificationsConsumer = pushNotificationsConsumer;
            _logger = logManager.GetLogger<IosPushNotificationService>();

            UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate(pushNotificationsConsumer, logManager);
            UNUserNotificationCenter.Current.SetNotificationCategories(new NSSet<UNNotificationCategory>(pushNotificationsConsumer.GetCategories().ToArray()));
        }

        /// <inheritdoc />
        public async Task RegisterForPushNotificationsAsync()
        {
            var requiredAuthorizationOptions = _pushNotificationsConsumer.GetRequiredAuthorizationOptions();
            var (isGranted, _) = await UNUserNotificationCenter.Current.RequestAuthorizationAsync(requiredAuthorizationOptions);
            _pushNotificationsConsumer.OnPushNotificationAuthorizationResult(isGranted);

            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            });
        }

        /// <inheritdoc />
        public async Task UnregisterForPushNotificationsAsync()
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.UnregisterForRemoteNotifications();
            });

            await _pushNotificationsConsumer.OnUnregisterFromPushNotifications();
        }

        /// <inheritdoc />
        public void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            _pushNotificationsConsumer.OnRegisteredForRemoteNotifications(application, deviceToken);
        }

        /// <inheritdoc />
        public void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            _pushNotificationsConsumer.OnFailedToRegisterForRemoteNotifications(application, error);
        }

        /// <inheritdoc />
        public void DidReceiveRemoteNotification(
            UIApplication application,
            NSDictionary userInfo,
            Action<UIBackgroundFetchResult> completionHandler)
        {
            if (_pushNotificationsConsumer.TryHandleRemoteNotification(application, userInfo, completionHandler))
            {
                return;
            }

            _logger.Warn("Notification have not been handled by consumer, invoking default handler");
            completionHandler.Invoke(UIBackgroundFetchResult.NoData);
        }
    }
}
