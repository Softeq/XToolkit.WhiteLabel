﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;
using BasePermission = Microsoft.Maui.ApplicationModel.Permissions.BasePermission;
using EssentialsPermissions = Microsoft.Maui.ApplicationModel.Permissions;

namespace Softeq.XToolkit.Permissions
{
    /// <inheritdoc cref="IPermissionsService" />
    public class PermissionsService : IPermissionsService
    {
        /// <inheritdoc />
        public Task<PermissionStatus> RequestPermissionsAsync<T>()
            where T : BasePermission, new()
        {
            return MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var result = await EssentialsPermissions.RequestAsync<T>().ConfigureAwait(false);
                return result.ToPermissionStatus();
            });
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
            MainThread.BeginInvokeOnMainThread(AppInfo.ShowSettingsUI);
        }

        public bool ShouldShowRationale<T>() where T : BasePermission, new()
        {
            return EssentialsPermissions.ShouldShowRationale<T>();
        }
    }
}
