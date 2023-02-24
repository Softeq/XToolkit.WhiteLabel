// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using BasePermission = Microsoft.Maui.ApplicationModel.Permissions.BasePermission;

namespace Softeq.XToolkit.Permissions
{
    /// <summary>
    ///     Service for customizing the dialogs, which are shown to the user
    ///     before permission requests (pre-notification dialogs).
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
