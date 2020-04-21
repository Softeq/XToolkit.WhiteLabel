// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Plugin.Permissions;

namespace Softeq.XToolkit.Permissions
{
    /// <summary>
    ///     Service for customizing the pre-notification of the user before permissions request.
    /// </summary>
    public interface IPermissionsDialogService
    {
        /// <summary>
        ///     Gets confirmation from the user about the permission.
        /// </summary>
        /// <typeparam name="T">Type of permission.</typeparam>
        /// <returns><c>true</c> when confirmed. Permission can be requested.</returns>
        Task<bool> ConfirmPermissionAsync<T>() where T : BasePermission;

        /// <summary>
        ///     Gets confirmation from the user about opening settings.
        /// </summary>
        /// <typeparam name="T">Type of permission.</typeparam>
        /// <returns><c>true</c> when confirmed. Settings can be opened.</returns>
        Task<bool> ConfirmOpenSettingsForPermissionAsync<T>() where T : BasePermission;
    }
}
