using System.Threading.Tasks;

namespace Softeq.XToolkit.PushNotifications.Abstract
{
    public interface IPushTokenSynchronizer
    {
        /// <summary>
        ///     Callback called when push notification token has changed.
        /// </summary>
        /// <param name="token">New push notification token.</param>
        void OnTokenChanged(string token);

        /// <summary>
        ///     Callback called when application failed to register for push notifications.
        /// </summary>
        /// <returns>
        ///     A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task OnRegisterFailedInternalAsync();

        /// <summary>
        ///     Use this method for unregister from remote push notifications.
        /// </summary>
        /// <returns>
        ///     A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task UnregisterFromRemotePushNotificationsAsync();

        /// <summary>
        ///     This method resends push token to server if not registered.
        /// </summary>
        /// <returns>
        ///     A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task SynchronizeTokenIfNeededAsync();
    }
}
