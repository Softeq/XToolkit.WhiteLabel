using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Permissions;

namespace Playground
{
    public class PermissionsDialogService : IPermissionsDialogService
    {
        public Task<bool> ConfirmOpenSettingsForPermissionAsync(Permission permission)
        {
            return Task.FromResult(true);
        }

        public Task<bool> ConfirmPermissionAsync(Permission permission)
        {
            return Task.FromResult(true);
        }
    }
}
