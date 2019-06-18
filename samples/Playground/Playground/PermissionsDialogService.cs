using System;
using System.Threading.Tasks;
using Plugin.Permissions;
using Softeq.XToolkit.Permissions;

namespace Playground
{
    public class PermissionsDialogService : IPermissionsDialogService
    {
        public Task<bool> ConfirmOpenSettingsForPermissionAsync<T>()
             where T : BasePermission
        {
            return Task.FromResult(true);
        }

        public Task<bool> ConfirmPermissionAsync<T>()
             where T : BasePermission
        {
            return Task.FromResult(true);
        }
    }
}
