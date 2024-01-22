// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using BasePermission = Microsoft.Maui.ApplicationModel.Permissions.BasePermission;
using CustomPermissions = Softeq.XToolkit.Permissions.Permissions;
using PlatformCustomPermissions = Softeq.XToolkit.Permissions.iOS.Permissions;

namespace Softeq.XToolkit.Permissions.iOS
{
    /// <inheritdoc cref="IPermissionsManager" />
    public class PermissionsManager : IPermissionsManager
    {
        private readonly string _isNotificationsPermissionRequestedKey =
            $"{nameof(PermissionsManager)}_{nameof(IsNotificationsPermissionRequested)}";

        private readonly IPermissionsService _permissionsService;

        private IPermissionsDialogService _permissionsDialogService;

        public PermissionsManager(
            IPermissionsService permissionsService)
        {
            _permissionsService = permissionsService;
            _permissionsDialogService = new DefaultPermissionsDialogService();
        }

        private bool IsNotificationsPermissionRequested
        {
            get => Preferences.Get(_isNotificationsPermissionRequestedKey, false);
            set => Preferences.Set(_isNotificationsPermissionRequestedKey, value);
        }

        /// <inheritdoc />
        public virtual Task<PermissionStatus> CheckWithRequestAsync<T>()
            where T : BasePermission, new()
        {
            var permissionType = typeof(T);

            if (permissionType == typeof(CustomPermissions.Notifications))
            {
                return CommonCheckWithRequestAsync<PlatformCustomPermissions.Notifications>();
            }
            else if (permissionType == typeof(CustomPermissions.Bluetooth))
            {
                return CommonCheckWithRequestAsync<PlatformCustomPermissions.Bluetooth>();
            }
            else
            {
                return CommonCheckWithRequestAsync<T>();
            }
        }

        /// <inheritdoc />
        public Task<PermissionStatus> CheckAsync<T>()
            where T : BasePermission, new()
        {
            var permissionType = typeof(T);

            if (permissionType == typeof(CustomPermissions.Notifications))
            {
                return _permissionsService.CheckPermissionsAsync<PlatformCustomPermissions.Notifications>();
            }
            else if (permissionType == typeof(CustomPermissions.Bluetooth))
            {
                return _permissionsService.CheckPermissionsAsync<PlatformCustomPermissions.Bluetooth>();
            }
            else
            {
                return _permissionsService.CheckPermissionsAsync<T>();
            }
        }

        /// <inheritdoc />
        public void SetPermissionDialogService(IPermissionsDialogService permissionsDialogService)
        {
            _permissionsDialogService = permissionsDialogService ??
                throw new ArgumentNullException(nameof(permissionsDialogService));
        }

        private void OpenSettings()
        {
            _permissionsService.OpenSettings();
        }

        private async Task<PermissionStatus> CommonCheckWithRequestAsync<T>()
            where T : BasePermission, new()
        {
            var permissionStatus = await _permissionsService.CheckPermissionsAsync<T>().ConfigureAwait(false);
            if (permissionStatus == PermissionStatus.Granted)
            {
                return permissionStatus;
            }

            if (permissionStatus == PermissionStatus.Denied)
            {
                await OpenSettingsWithConfirmationAsync<T>().ConfigureAwait(false);
            }

            if (permissionStatus == PermissionStatus.Unknown)
            {
                var confirmationResult = await _permissionsDialogService.ConfirmPermissionAsync<T>().ConfigureAwait(false);
                if (confirmationResult)
                {
                    permissionStatus = await _permissionsService.RequestPermissionsAsync<T>().ConfigureAwait(false);
                }
            }

            return permissionStatus;
        }

        private async Task<PermissionStatus> OpenSettingsWithConfirmationAsync<T>()
            where T : BasePermission
        {
            var openSettingsConfirmed = await _permissionsDialogService
                .ConfirmOpenSettingsForPermissionAsync<T>().ConfigureAwait(false);
            if (openSettingsConfirmed)
            {
                OpenSettings();
            }

            return PermissionStatus.Unknown;
        }
    }
}
