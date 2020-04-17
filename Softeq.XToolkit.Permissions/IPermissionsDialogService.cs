// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Plugin.Permissions;

namespace Softeq.XToolkit.Permissions
{
    /// <summary>
    ///     Service for customizing the pre-notification of the user before request permissions.
    /// </summary>
    public interface IPermissionsDialogService
    {
        /// <summary>
        ///     Gets confirmation from the user about the permission.
        /// </summary>
        /// <typeparam name="T">Type of permission.</typeparam>
        /// <returns><c>true</c> for continue.</returns>
        Task<bool> ConfirmPermissionAsync<T>() where T : BasePermission;

        /// <summary>
        ///     Gets confirmation from the user about opening settings.
        /// </summary>
        /// <typeparam name="T">Type of permission.</typeparam>
        /// <returns><c>true</c> for continue.</returns>
        Task<bool> ConfirmOpenSettingsForPermissionAsync<T>() where T : BasePermission;
    }
}
