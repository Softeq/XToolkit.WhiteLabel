// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Android.OS;
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
            var result = await EssentialsPermissions.RequestAsync<T>().ConfigureAwait(false);
            return result.ToPermissionStatus();
        }

        /// <inheritdoc />
        public async Task<PermissionStatus> CheckPermissionsAsync<T>()
            where T : BasePermission, new()
        {
            var result = await EssentialsPermissions.CheckStatusAsync<T>().ConfigureAwait(false);
            return result.ToPermissionStatus();
        }

        /// <inheritdoc />
        public void OpenSettings()
        {
            RunInMainThread(Xamarin.Essentials.AppInfo.ShowSettingsUI);
        }

        private static void RunInMainThread(Action action)
        {
            if (Looper.MainLooper == Looper.MyLooper())
            {
                action();
            }
            else
            {
                new Handler(Looper.MainLooper).Post(action);
            }
        }
    }
}
