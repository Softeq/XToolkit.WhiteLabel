// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;

namespace Softeq.XToolkit.PushNotifications
{
    public interface IPushNotificationsService : IPushNotificationsReceiver
    {
        #region Initialization

        /// <summary>
        ///     This method should be called on application start to initialize work with push notifications.
        ///     <para/>
        ///     On iOS call this method before your app finishes launching (in WillFinishLaunching or FinishedLaunching of AppDelegate).
        ///     <para/>
        ///     On Android call this method in OnCreate of Application.
        /// </summary>
        /// <param name="showForegroundNotificationsInSystemOptions">
        ///     If <see cref="ForegroundNotificationOptions.Show"/> or <see cref="ForegroundNotificationOptions.ShowWithBadge"/>:
        ///     <para/>
        ///     Notifications received in foreground will be shown in system as well as all other notifications.
        ///     <para/>
        ///     If <see cref="ForegroundNotificationOptions.ShowWithBadge"/>:
        ///     <para/>
        ///     1. On iOS: badge value from the notification will be applied.
        ///     <para/>
        ///     2. On Android: no effect for now (= Show).
        /// </param>
        void Initialize(ForegroundNotificationOptions showForegroundNotificationsInSystemOptions);

        #endregion

        #region Removing notifications

        /// <summary>
        ///     Clears all notifications from notifications center.
        /// </summary>
        void ClearAllNotifications();

        #endregion

        #region Changing badge number

        /// <summary>
        ///     Set specific value for the app Badge number manually. Negative or zero value will remove the badge.
        /// </summary>
        /// <param name="badgeNumber">Number to be displayed on the Badge.</param>
        /// <remarks>
        ///     On Android <see href="https://github.com/wcoder/ShortcutBadger"/> library is used.
        ///     See wiki for details, not all devices are supported.
        /// </remarks>
        void SetBadgeNumber(int badgeNumber);

        #endregion

        #region Registration

        /// <summary>
        ///     Registers application for push notifications.
        /// </summary>
        void RegisterForPushNotifications();

        /// <summary>
        ///     Registers application for push notifications asynchronously.
        /// </summary>
        /// <returns>
        ///     Task with <see cref="PushNotificationRegistrationResult" /> about registration status in system and on server.
        /// </returns>
        Task<PushNotificationRegistrationResult> RegisterForPushNotificationsAsync();

        /// <summary>
        ///     Callback to notify the service that registration for push notifications finished successfully.
        ///     <para/>
        ///     On iOS should be called in AppDelegate's RegisteredForRemoteNotifications.
        ///     <para/>
        ///     On Android is called internally.
        /// </summary>
        /// <param name="token">Push notifications token.</param>
        void OnRegisteredForPushNotifications(string token);

        /// <summary>
        ///     Callback to notify the service that registration for push notifications failed.
        ///     <para/>
        ///     On iOS should be called in AppDelegate's FailedToRegisterForRemoteNotifications.
        ///     <para/>
        ///     On Android is called internally.
        /// </summary>
        /// <param name="errorMessage">Error Message.</param>
        void OnFailedToRegisterForPushNotifications(string errorMessage);

        /// <summary>
        ///     Unregisters application from push notifications.
        /// </summary>
        /// <param name="unregisterInSystem">
        ///     If <see langword="true"/> application will be unregistered from push notifications inside OS system.
        ///     <para/>
        ///     Default value is <see langword="false"/> as it is not recommended to do by Apple and Firebase documentation,
        ///     but it might be useful on logout.
        ///     <para/>
        ///     On Android unsubscribing in system will only work when there is Internet Connection.
        /// </param>
        /// <returns>
        ///     Task with <see cref="PushNotificationsUnregisterResult" /> about unregistration status on server and in system as well.
        /// </returns>
        [Obsolete("Use UnregisterFromPushNotifications with PushNotificationsUnregisterOptions param instead.")]
        Task<PushNotificationsUnregisterResult> UnregisterFromPushNotifications(bool unregisterInSystem = false);

        /// <summary>
        ///     Unregisters application from push notifications.
        /// </summary>
        /// <param name="options">
        ///     Defines if we should unregister in system and if we should unregister on server.
        ///     <para/>
        ///     It is not recommended to unsubscribe in system by Apple and Firebase documentation, but it might be useful on logout.
        ///     <para/>
        ///     On Android unsubscribing in system will only work when there is Internet Connection.
        /// </param>
        /// <returns>Task with <see cref="PushNotificationsUnregisterResult" /> about unregistration status on server and in system as well.</returns>
        Task<PushNotificationsUnregisterResult> UnregisterFromPushNotifications(PushNotificationsUnregisterOptions options);

        #endregion

        #region Receiving messages

        // Receiving messages - inhereted from IPushNotificationsReceiver

        #endregion
    }
}
