// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Softeq.XToolkit.Permissions.Droid
{
    public class PermissionsManager : IPermissionsManager
    {
        private readonly string _isPermissionRequestedKey = $"{nameof(PermissionsManager)}_{nameof(IsPermissionRequested)}";
        
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
        
        private bool IsPermissionRequested
        {
            get => _internalSettings.GetValueOrDefault(_isPermissionRequestedKey, false);
            set => _internalSettings.AddOrUpdateValue(_isPermissionRequestedKey, value);
        }

        public Task<PermissionStatus> CheckWithRequestAsync(Permission permission)
        {
            return CommonCheckWithRequestAsync(permission);
        }

        public Task<PermissionStatus> CheckAsync(Permission permission)
        {
            return _permissionsService.CheckPermissionsAsync(permission);
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

            if (permissionStatus == PermissionStatus.Denied && IsPermissionRequested)
            {
                await OpenSettingsWithConfirmationAsync(permission).ConfigureAwait(false);
                return PermissionStatus.Denied;
            }

            var confirmationResult = await _permissionsDialogService.ConfirmPermissionAsync(permission).ConfigureAwait(false);
            if (confirmationResult)
            {
                permissionStatus = await _permissionsService.RequestPermissionsAsync(permission).ConfigureAwait(false);
                
                IsPermissionRequested = true;
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
