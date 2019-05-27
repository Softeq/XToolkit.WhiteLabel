// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Softeq.XToolkit.Permissions.iOS
{
    public class PermissionsManager : IPermissionsManager
    {
        private readonly string _isNotificationsPermissionRequestedKey =
            $"{nameof(PermissionsManager)}_{nameof(IsNotificationsPermissionRequested)}";

        private readonly IPermissionsService _permissionsService;
        private readonly IPermissionsDialogService _permissionsDialogService;
        private readonly ISettings _internalSettings;

        public PermissionsManager(
            IPermissionsService permissionsService,
            IPermissionsDialogService permissionsDialogService)
        {
            _permissionsService = permissionsService;
            _permissionsDialogService = permissionsDialogService;
            _internalSettings = CrossSettings.Current;
        }

        private bool IsNotificationsPermissionRequested
        {
            get => _internalSettings.GetValueOrDefault(_isNotificationsPermissionRequestedKey, false);
            set => _internalSettings.AddOrUpdateValue(_isNotificationsPermissionRequestedKey, value);
        }

        public Task<PermissionStatus> CheckWithRequestAsync(Permission permission)
        {
            return permission == Permission.Notifications
                ? NotificationsCheckWithRequestAsync()
                : CommonCheckWithRequestAsync(permission);
        }

        public Task<PermissionStatus> CheckAsync(Permission permission)
        {
            return _permissionsService.CheckPermissionsAsync(permission);
        }

        private void OpenSettings()
        {
            _permissionsService.OpenSettings();
        }

        private async Task<PermissionStatus> NotificationsCheckWithRequestAsync()
        {
            if (!IsNotificationsPermissionRequested)
            {
                var isConfirmed = await _permissionsDialogService.ConfirmPermissionAsync(Permission.Notifications).ConfigureAwait(false);
                if (!isConfirmed)
                {
                    return PermissionStatus.Denied;
                }
            }

            var permissionStatus = await _permissionsService.RequestPermissionsAsync(Permission.Notifications).ConfigureAwait(false);
            
            if (IsNotificationsPermissionRequested && permissionStatus != PermissionStatus.Granted)
            {
                permissionStatus = await OpenSettingsWithConfirmationAsync(Permission.Notifications).ConfigureAwait(false);
            }
            
            IsNotificationsPermissionRequested = true;

            return permissionStatus;
        }

        private async Task<PermissionStatus> CommonCheckWithRequestAsync(Permission permission)
        {
            var permissionStatus = await _permissionsService.CheckPermissionsAsync(permission).ConfigureAwait(false);
            if (permissionStatus == PermissionStatus.Granted)
            {
                return permissionStatus;
            }

            if (permissionStatus == PermissionStatus.Denied)
            {
                await OpenSettingsWithConfirmationAsync(permission).ConfigureAwait(false);
            }

            if (permissionStatus == PermissionStatus.Unknown)
            {
                var confirmationResult = await _permissionsDialogService.ConfirmPermissionAsync(permission).ConfigureAwait(false);
                if (confirmationResult)
                {
                    permissionStatus = await _permissionsService.RequestPermissionsAsync(permission).ConfigureAwait(false);
                }
            }

            return permissionStatus;
        }

        private async Task<PermissionStatus> OpenSettingsWithConfirmationAsync(Permission permission)
        {
            var openSettingsConfirmed = await _permissionsDialogService
                .ConfirmOpenSettingsForPermissionAsync(permission).ConfigureAwait(false);
            if (openSettingsConfirmed)
            {
                OpenSettings();
            }

            return PermissionStatus.Unknown;
        }
    }
}