// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using UserNotifications;

namespace Softeq.XToolkit.PushNotifications.iOS.Abstract
{
    /// <summary>
    ///     Basic interface for push notification consumer on iOS platform.
    /// </summary>
    public interface IIosPushNotificationsConsumer
    {
        UNAuthorizationOptions GetRequiredAuthorizationOptions();

        /// <summary>
        ///     Gets collection of Notification Categories that need to be registered for the app.
        /// </summary>
        /// <returns>
        ///     Enumeration of <see cref="UNNotificationCategory"/> to be registered.
        /// </returns>
        IEnumerable<UNNotificationCategory> GetCategories();

        /// <summary>
        ///    Callback called when push notification need to be presented.
        /// </summary>
        /// <param name="center">Current Notification Center.</param>
        /// <param name="notification">Received notification.</param>
        /// <param name="completionHandler">Callback to be called to present notification.</param>
        /// <returns><see langword="true"/> if consumer has handled notification, <see langword="false"/> otherwise.</returns>
        /// <remarks>
        ///     <paramref name="completionHandler"/> should always be called if method returns <see langword="true"/>,
        ///     and should not be called if method returns <see langword="false"/>.
        /// </remarks>
        bool TryPresentNotification(
            UNUserNotificationCenter center,
            UNNotification notification,
            Action<UNNotificationPresentationOptions> completionHandler);

        /// <summary>
        ///    Callback called when user response to push notification.
        /// </summary>
        /// <param name="center">Current Notification Center.</param>
        /// <param name="response">Received notification response.</param>
        /// <param name="completionHandler">Callback to be called when response is handled.</param>
        /// <returns>
        ///     <see langword="true"/> if consumer has handled notification response, <see langword="false"/> otherwise.
        /// </returns>
        /// <remarks>
        ///     <paramref name="completionHandler"/> should always be called if method returns <see langword="true"/>,
        ///     and should not be called if method returns <see langword="false"/>.
        /// </remarks>
        bool TryHandleNotificationResponse(
            UNUserNotificationCenter center,
            UNNotificationResponse response,
            Action completionHandler);

        /// <summary>
        ///    Callback called when push notification received.
        /// </summary>
        /// <param name="application">Current application.</param>
        /// <param name="userInfo">Payload of received notification.</param>
        /// <param name="completionHandler">Callback to be called when notification is handled.</param>
        /// <returns><see langword="true"/> if consumer has handled notification, <see langword="false"/> otherwise.</returns>
        /// <remarks>
        ///     <paramref name="completionHandler"/> should always be called if method returns <see langword="true"/>,
        ///     and should not be called if method returns <see langword="false"/>.
        /// </remarks>
        bool TryHandleRemoteNotification(
            UIApplication application,
            NSDictionary userInfo,
            Action<UIBackgroundFetchResult> completionHandler);

        /// <summary>
        ///     Callback called after push notification authorization by user.
        /// </summary>
        /// <param name="isGranted">
        ///     <see langword="true"/> if user authorized push notifications, <see langword="false"/> otherwise.
        /// </param>
        void OnPushNotificationAuthorizationResult(bool isGranted);

        /// <summary>
        ///     Callback called when push notification token has been registered.
        /// </summary>
        /// <param name="application">Current application.</param>
        /// <param name="deviceToken">Registered push notification token.</param>
        void OnRegisteredForRemoteNotifications(UIApplication application, NSData deviceToken);

        /// <summary>
        ///     Callback called when push notification token failed to be registered.
        /// </summary>
        /// <param name="application">Current application.</param>
        /// <param name="error">Error which occured during registration.</param>
        void OnFailedToRegisterForRemoteNotifications(UIApplication application, NSError error);

        /// <summary>
        ///     Callback called when application has unregistered from push notifications.
        /// </summary>
        /// <returns>
        ///     A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task OnUnregisterFromPushNotifications();
    }
}
