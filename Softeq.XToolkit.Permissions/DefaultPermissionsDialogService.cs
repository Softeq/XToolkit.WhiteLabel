// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Plugin.Permissions;

namespace Softeq.XToolkit.Permissions
{
    /// <inheritdoc cref="IPermissionsDialogService"/>
    public class DefaultPermissionsDialogService : IPermissionsDialogService
    {
        /// <inheritdoc />
        public virtual Task<bool> ConfirmPermissionAsync<T>() where T : BasePermission
        {
            return Task.FromResult(true);
        }

        /// <inheritdoc />
        public virtual Task<bool> ConfirmOpenSettingsForPermissionAsync<T>() where T : BasePermission
        {
            return Task.FromResult(true);
        }
    }
}
