// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Xamarin.Essentials;
using BasePermission = Xamarin.Essentials.Permissions.BasePermission;
using EssentialsPermissions = Xamarin.Essentials.Permissions;

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
                Xamarin.Essentials.PermissionStatus result;
                if (typeof(T) == typeof(NotificationsPermission))
                {
                    result = await EssentialsPermissions
                        .RequestAsync<NotificationsPlatformPermission>()
                        .ConfigureAwait(false);
                }
                else
                {
                    result = await EssentialsPermissions
                        .RequestAsync<T>()
                        .ConfigureAwait(false);
                }

                return result.ToPermissionStatus();
            });
        }

        /// <inheritdoc />
        public async Task<PermissionStatus> CheckPermissionsAsync<T>()
            where T : BasePermission, new()
        {
            Xamarin.Essentials.PermissionStatus result;
            if (typeof(T) == typeof(NotificationsPermission))
            {
                result = await EssentialsPermissions
                    .CheckStatusAsync<NotificationsPlatformPermission>()
                    .ConfigureAwait(false);
            }
            else
            {
                result = await EssentialsPermissions
                    .CheckStatusAsync<T>()
                    .ConfigureAwait(false);
            }

            return result.ToPermissionStatus();
        }

        /// <inheritdoc />
        public void OpenSettings()
        {
            MainThread.BeginInvokeOnMainThread(AppInfo.ShowSettingsUI);
        }

        public bool ShouldShowRationale<T>() where T : BasePermission, new()
        {
            return typeof(T) == typeof(NotificationsPermission)
                ? EssentialsPermissions.ShouldShowRationale<NotificationsPlatformPermission>()
                : EssentialsPermissions.ShouldShowRationale<T>();
        }
    }
}
