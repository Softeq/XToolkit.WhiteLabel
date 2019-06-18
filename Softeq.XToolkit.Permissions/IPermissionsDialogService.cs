// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Plugin.Permissions;

namespace Softeq.XToolkit.Permissions
{
    public interface IPermissionsDialogService
    {
        Task<bool> ConfirmPermissionAsync<T>() where T : BasePermission;
        Task<bool> ConfirmOpenSettingsForPermissionAsync<T>() where T : BasePermission;
    }
}
