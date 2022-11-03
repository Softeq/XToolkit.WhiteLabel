// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.PushNotifications.Abstract
{
    public interface IPushNotificationsService
    {
        /// <summary>
        ///     Registers application for push notifications asynchronously.
        /// </summary>
        /// <returns>
        ///     A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task RegisterForPushNotificationsAsync();

        /// <summary>
        ///     Unregisters application from push notifications asynchronously.
        /// </summary>
        /// <returns>
        ///     A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task UnregisterForPushNotificationsAsync();
    }
}
