// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using UIKit;

namespace Softeq.XToolkit.PushNotifications.iOS.Abstract
{
    /// <summary>
    ///     Basic set of callbacks, received by AppDelegate while working with push notifications.
    /// </summary>
    public interface IPushNotificationAppDelegate
    {
        /// <summary>
        ///     Callback called when push notification token has been registered.
        /// </summary>
        /// <param name="application">Current application.</param>
        /// <param name="deviceToken">Registered push notification token.</param>
        void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken);

        /// <summary>
        ///     Callback called when push notification token failed to be registered.
        /// </summary>
        /// <param name="application">Current application.</param>
        /// <param name="error">Error which occured during registration.</param>
        void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error);

        /// <summary>
        ///    Callback called when push notification received.
        /// </summary>
        /// <param name="application">Current application.</param>
        /// <param name="userInfo">Payload of received notification.</param>
        /// <param name="completionHandler">Callback to be called when notification is handled.</param>
        void DidReceiveRemoteNotification(
            UIApplication application,
            NSDictionary userInfo,
            Action<UIBackgroundFetchResult> completionHandler);
    }
}
