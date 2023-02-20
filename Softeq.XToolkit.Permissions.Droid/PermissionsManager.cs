// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using EssentialsBasePermission = Xamarin.Essentials.Permissions.BasePermission;
using XToolkitPermissions = Softeq.XToolkit.Permissions;
using XToolkitPermissionsDroid = Softeq.XToolkit.Permissions.Droid.Permissions;

namespace Softeq.XToolkit.Permissions.Droid
{
    /// <inheritdoc cref="IPermissionsManager" />
    public class PermissionsManager : IPermissionsManager
    {
        private readonly TimeSpan _showPermissionDialogThreshold = TimeSpan.FromMilliseconds(200);
        private readonly IPermissionsService _permissionsService;

        private IPermissionsDialogService _permissionsDialogService;

        public PermissionsManager(
            IPermissionsService permissionsService)
        {
            _permissionsService = permissionsService;
            _permissionsDialogService = new DefaultPermissionsDialogService();
        }

        /// <inheritdoc />
        public virtual Task<PermissionStatus> CheckAsync<T>()
            where T : EssentialsBasePermission, new()
        {
            var permissionType = typeof(T);
            if (permissionType == typeof(XToolkitPermissions.Notifications))
            {
                return _permissionsService.CheckPermissionsAsync<XToolkitPermissionsDroid.Notifications>();
            }
            else if (permissionType == typeof(XToolkitPermissions.Bluetooth))
            {
                return _permissionsService.CheckPermissionsAsync<XToolkitPermissionsDroid.Bluetooth>();
            }
            else
            {
                return _permissionsService.CheckPermissionsAsync<T>();
            }
        }

        /// <inheritdoc />
        public Task<PermissionStatus> CheckWithRequestAsync<T>()
            where T : EssentialsBasePermission, new()
        {
            var permissionType = typeof(T);
            if (permissionType == typeof(XToolkitPermissions.Notifications))
            {
                return CommonCheckWithRequestAsync<XToolkitPermissionsDroid.Notifications>();
            }
            else if (permissionType == typeof(XToolkitPermissions.Bluetooth))
            {
                return CommonCheckWithRequestAsync<XToolkitPermissionsDroid.Bluetooth>();
            }
            else
            {
                return CommonCheckWithRequestAsync<T>();
            }
        }

        /// <inheritdoc />
        public void SetPermissionDialogService(IPermissionsDialogService permissionsDialogService)
        {
            _permissionsDialogService = permissionsDialogService
                ?? throw new ArgumentNullException(nameof(permissionsDialogService));
        }

        private void OpenSettings()
        {
            _permissionsService.OpenSettings();
        }

        private void RemoveOldKeys<T>()
            where T : EssentialsBasePermission, new()
        {
            var requestedKey = GetPermissionRequestedKey<T>();
            if (Preferences.ContainsKey(requestedKey))
            {
                Preferences.Remove(requestedKey);
            }

            var deniedEverKey = GetPermissionDeniedEverKey<T>();
            if (Preferences.ContainsKey(deniedEverKey))
            {
                Preferences.Remove(deniedEverKey);
            }

            string GetPermissionRequestedKey<TPermission>()
                where TPermission : EssentialsBasePermission
            {
                return $"{nameof(PermissionsManager)}_IsPermissionRequested_{typeof(T).Name}";
            }

            string GetPermissionDeniedEverKey<T>()
                where T : EssentialsBasePermission
            {
                return $"{nameof(PermissionsManager)}_IsPermissionDeniedEver_{typeof(T).Name}";
            }
        }

        private async Task<PermissionStatus> CommonCheckWithRequestAsync<T>()
            where T : EssentialsBasePermission, new()
        {
            var permissionStatus = await _permissionsService.CheckPermissionsAsync<T>().ConfigureAwait(false);
            if (permissionStatus == PermissionStatus.Granted)
            {
                return permissionStatus;
            }

            RemoveOldKeys<T>();

            var timer = new Stopwatch();
            timer.Start();

            var confirmationResult = await _permissionsDialogService.ConfirmPermissionAsync<T>().ConfigureAwait(false);
            if (confirmationResult)
            {
                permissionStatus = await _permissionsService.RequestPermissionsAsync<T>().ConfigureAwait(false);
            }

            if (permissionStatus == PermissionStatus.Denied
                && timer.Elapsed < _showPermissionDialogThreshold)
            {
                await OpenSettingsWithConfirmationAsync<T>().ConfigureAwait(false);
                return PermissionStatus.Denied;
            }

            return permissionStatus;
        }

        private async Task OpenSettingsWithConfirmationAsync<T>()
            where T : EssentialsBasePermission
        {
            var openSettingsConfirmed = await _permissionsDialogService
                .ConfirmOpenSettingsForPermissionAsync<T>().ConfigureAwait(false);
            if (openSettingsConfirmed)
            {
                OpenSettings();
            }
        }
    }
}