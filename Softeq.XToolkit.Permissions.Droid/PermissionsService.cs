// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using BasePermission = Xamarin.Essentials.Permissions.BasePermission;
using EssentialsPermissions = Xamarin.Essentials.Permissions;
using PluginPermissionStatus = Xamarin.Essentials.PermissionStatus;

namespace Softeq.XToolkit.Permissions.Droid
{
    /// <inheritdoc cref="IPermissionsService" />
    public class PermissionsService : IPermissionsService
    {
        /// <inheritdoc />
        public async Task<PermissionStatus> RequestPermissionsAsync<T>()
            where T : BasePermission, new()
        {
            return await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var result = await EssentialsPermissions.RequestAsync<T>().ConfigureAwait(false);
                return ToPermissionStatus(result);
            });
        }

        /// <inheritdoc />
        public async Task<PermissionStatus> CheckPermissionsAsync<T>()
            where T : BasePermission, new()
        {
            var result = await EssentialsPermissions.CheckStatusAsync<T>().ConfigureAwait(false);
            return ToPermissionStatus(result);
        }

        /// <inheritdoc />
        public void OpenSettings()
        {
            MainThread.BeginInvokeOnMainThread(AppInfo.ShowSettingsUI);
        }

        public bool ShouldShowRationale<T>() where T : BasePermission, new()
        {
            return EssentialsPermissions.ShouldShowRationale<T>();
        }

        private static PermissionStatus ToPermissionStatus(PluginPermissionStatus permissionStatus)
        {
            return permissionStatus switch
            {
                PluginPermissionStatus.Denied => PermissionStatus.Denied,
                PluginPermissionStatus.Disabled => PermissionStatus.Denied,
                PluginPermissionStatus.Granted => PermissionStatus.Granted,
                PluginPermissionStatus.Restricted => PermissionStatus.Restricted,
                PluginPermissionStatus.Unknown => PermissionStatus.Unknown,
                _ => throw new InvalidEnumArgumentException(
                    nameof(permissionStatus),
                    (int) permissionStatus,
                    permissionStatus.GetType())
            };
        }
    }
}
