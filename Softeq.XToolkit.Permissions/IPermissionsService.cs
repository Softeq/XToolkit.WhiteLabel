// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.Permissions
{
    public interface IPermissionsService
    {
        Task<PermissionStatus> RequestPermissionsAsync(Permission permission);
        Task<PermissionStatus> CheckPermissionsAsync(Permission permission);
        void OpenSettings();
    }
}
