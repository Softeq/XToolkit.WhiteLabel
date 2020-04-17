// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Plugin.Permissions;

namespace Softeq.XToolkit.Permissions
{
    /// <summary>
    ///     Platform-specific logic for check/request permissions
    ///     (for use Permissions.Plugin or custom implementation of permissions request).
    /// </summary>
    public interface IPermissionsService
    {
        /// <summary>
        ///     Requests the permissions from the users.
        /// </summary>
        /// <typeparam name="T">Type of permission.</typeparam>
        /// <returns>The permissions and their status.</returns>
        Task<PermissionStatus> RequestPermissionsAsync<T>() where T : BasePermission, new();

        /// <summary>
        ///     Determines whether this permission has been granted.
        /// </summary>
        /// <typeparam name="T">Type of permission.</typeparam>
        /// <returns>The permission status for requested permission.</returns>
        Task<PermissionStatus> CheckPermissionsAsync<T>() where T : BasePermission, new();

        /// <summary>
        ///     Attempts to open the app settings to adjust the permissions.
        /// </summary>
        void OpenSettings();
    }
}
