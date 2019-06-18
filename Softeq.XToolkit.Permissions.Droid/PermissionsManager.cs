// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Softeq.XToolkit.Permissions.Droid
{
    public class PermissionsManager : IPermissionsManager
    {
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

        public Task<PermissionStatus> CheckWithRequestAsync(Permission permission)
        {
            return CommonCheckWithRequestAsync(permission);
        }

        public Task<PermissionStatus> CheckAsync(Permission permission)
        {
            return _permissionsService.CheckPermissionsAsync(permission);
        }

        private bool IsPermissionRequested(Permission permission)
        {
            return _internalSettings.GetValueOrDefault(GetPermissionRequestedKey(permission), false);
        }

        private void SetPermissionRequested(Permission permission, bool value)
        {
            _internalSettings.AddOrUpdateValue(GetPermissionRequestedKey(permission), value);
        }

        private string GetPermissionRequestedKey(Permission permission)
        {
            return $"{nameof(PermissionsManager)}_IsPermissionRequested_{permission}";
        }

        private void OpenSettings()
        {
            _permissionsService.OpenSettings();
        }

        private async Task<PermissionStatus> CommonCheckWithRequestAsync(Permission permission)
        {
            var permissionStatus = await _permissionsService.CheckPermissionsAsync(permission).ConfigureAwait(false);
            if (permissionStatus == PermissionStatus.Granted)
            {
                return permissionStatus;
            }

            if (permissionStatus == PermissionStatus.Denied && IsPermissionRequested(permission))
            {
                await OpenSettingsWithConfirmationAsync(permission).ConfigureAwait(false);
                return PermissionStatus.Denied;
            }

            var confirmationResult = await _permissionsDialogService.ConfirmPermissionAsync(permission).ConfigureAwait(false);
            if (confirmationResult)
            {
                permissionStatus = await _permissionsService.RequestPermissionsAsync(permission).ConfigureAwait(false);

                SetPermissionRequested(permission, true);
            }

            return permissionStatus;
        }

        private async Task OpenSettingsWithConfirmationAsync(Permission permission)
        {
            var openSettingsConfirmed = await _permissionsDialogService
                .ConfirmOpenSettingsForPermissionAsync(permission).ConfigureAwait(false);
            if (openSettingsConfirmed)
            {
                OpenSettings();
            }
        }
    }
}
