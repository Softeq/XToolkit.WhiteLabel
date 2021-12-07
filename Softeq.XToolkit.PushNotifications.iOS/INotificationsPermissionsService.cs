// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.PushNotifications.iOS
{
    public interface INotificationsPermissionsService
    {
        /// <summary>
        ///     Requests push notifications permissions.
        /// </summary>
        /// <returns>Task with true value if permissions were granted, and false value otherwise.</returns>
        Task<bool> RequestNotificationsPermissions();
    }
}
