﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Android.OS;
using Plugin.Permissions;
using PluginPermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;

namespace Softeq.XToolkit.Permissions.Droid
{
    /// <inheritdoc cref="IPermissionsService" />
    public class PermissionsService : IPermissionsService
    {
        /// <inheritdoc />
        public async Task<PermissionStatus> RequestPermissionsAsync<T>()
            where T : BasePermission, new()
        {
            var result = await CrossPermissions.Current.RequestPermissionAsync<T>().ConfigureAwait(false);
            return result.ToPermissionStatus();
        }

        /// <inheritdoc />
        public async Task<PermissionStatus> CheckPermissionsAsync<T>()
            where T : BasePermission, new()
        {
            var result = await CrossPermissions.Current.CheckPermissionStatusAsync<T>().ConfigureAwait(false);
            return result.ToPermissionStatus();
        }

        /// <inheritdoc />
        public void OpenSettings()
        {
            RunInMainThread(() => { CrossPermissions.Current.OpenAppSettings(); });
        }

        private static void RunInMainThread(Action action)
        {
            if (Looper.MainLooper == Looper.MyLooper())
            {
                action();
            }
            else
            {
                var _ = new Handler(Looper.MainLooper).Post(action);
            }
        }
    }
}
