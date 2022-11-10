// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.PushNotifications.iOS.Abstract;
using UIKit;

namespace Softeq.XToolkit.PushNotifications.iOS.Services
{
    /// <summary>
    ///     Default implementation of <see cref="IPushNotificationAppDelegate"/> interface for iOS platform.
    /// </summary>
    public sealed class IosPushNotificationAppDelegate : IPushNotificationAppDelegate
    {
        private readonly IIosPushNotificationsConsumer _pushNotificationsConsumer;
        private readonly ILogger _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="IosPushNotificationAppDelegate"/> class.
        /// </summary>
        /// <param name="pushNotificationsConsumer">Consumer of the push notification related callbacks.</param>
        /// <param name="logManager">Provides logging.</param>
        public IosPushNotificationAppDelegate(
            IIosPushNotificationsConsumer pushNotificationsConsumer,
            ILogManager logManager)
        {
            _pushNotificationsConsumer = pushNotificationsConsumer;
            _logger = logManager.GetLogger<IosPushNotificationAppDelegate>();
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
