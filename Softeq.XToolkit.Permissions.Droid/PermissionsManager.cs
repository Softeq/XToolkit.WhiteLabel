// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Plugin.Permissions;
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

        public Task<PermissionStatus> CheckWithRequestAsync<T>()
            where T : BasePermission, new()
        {
            return CommonCheckWithRequestAsync<T>();
        }

        public Task<PermissionStatus> CheckAsync<T>()
            where T : BasePermission, new()
        {
            return _permissionsService.CheckPermissionsAsync<T>();
        }

        private bool IsPermissionRequested<T>()
            where T : BasePermission
        {
            return _internalSettings.GetValueOrDefault(GetPermissionRequestedKey<T>(), false);
        }

        private void SetPermissionRequested<T>(bool value)
            where T : BasePermission
        {
            _internalSettings.AddOrUpdateValue(GetPermissionRequestedKey<T>(), value);
        }

        private string GetPermissionRequestedKey<T>()
            where T : BasePermission
        {
            return $"{nameof(PermissionsManager)}_IsPermissionRequested_{typeof(T).Name}";
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

            if (permissionStatus == PermissionStatus.Denied && IsPermissionRequested<T>())
            {
                await OpenSettingsWithConfirmationAsync<T>().ConfigureAwait(false);
                return PermissionStatus.Denied;
            }

            var confirmationResult = await _permissionsDialogService.ConfirmPermissionAsync<T>().ConfigureAwait(false);
            if (confirmationResult)
            {
                permissionStatus = await _permissionsService.RequestPermissionsAsync<T>().ConfigureAwait(false);

                SetPermissionRequested<T>(true);
            }

            return permissionStatus;
        }

        private async Task OpenSettingsWithConfirmationAsync<T>()
            where T : BasePermission
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
