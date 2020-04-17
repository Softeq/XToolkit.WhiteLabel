// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Plugin.Permissions;

namespace Softeq.XToolkit.Permissions
{
    /// <summary>
    ///     Main interface for check/request permissions, for use in your logic.
    /// </summary>
    public interface IPermissionsManager
    {
        /// <summary>
        ///     Determines whether this permission has been granted.
        ///     Otherwise, uses dialog for the pre-notify user and then make requests permissions or opens Settings if needed.
        /// </summary>
        /// <typeparam name="T">Type of permission.</typeparam>
        /// <returns>The permissions and their status.</returns>
        Task<PermissionStatus> CheckWithRequestAsync<T>() where T : BasePermission, new();

        /// <summary>
        ///     Determines whether this permission has been granted.
        /// </summary>
        /// <typeparam name="T">Type of permission.</typeparam>
        /// <returns>The permission status for requested permission.</returns>
        Task<PermissionStatus> CheckAsync<T>() where T : BasePermission, new();

        /// <summary>
        ///     Sets dialog service for custom the pre-notification of the user.
        /// </summary>
        /// <param name="permissionsDialogService">Implementation of <see cref="IPermissionsDialogService"/>.</param>
        void SetPermissionDialogService(IPermissionsDialogService permissionsDialogService);
    }
}
