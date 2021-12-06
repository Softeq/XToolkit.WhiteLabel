// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using BasePermission = Xamarin.Essentials.Permissions.BasePermission;

namespace Softeq.XToolkit.Permissions.Droid
{
    /// <inheritdoc cref="IPermissionsManager" />
    public class PermissionsManager : IPermissionsManager
    {
        private readonly IPermissionsService _permissionsService;

        private IPermissionsDialogService _permissionsDialogService;

        public PermissionsManager(
            IPermissionsService permissionsService)
        {
            _permissionsService = permissionsService;
            _permissionsDialogService = new DefaultPermissionsDialogService();
        }

        /// <inheritdoc />
        public Task<PermissionStatus> CheckWithRequestAsync<T>()
            where T : BasePermission, new()
        {
            return CommonCheckWithRequestAsync<T>();
        }

        /// <inheritdoc />
        public Task<PermissionStatus> CheckAsync<T>()
            where T : BasePermission, new()
        {
            return _permissionsService.CheckPermissionsAsync<T>();
        }

        /// <inheritdoc />
        public void SetPermissionDialogService(IPermissionsDialogService permissionsDialogService)
        {
            _permissionsDialogService = permissionsDialogService
                                        ?? throw new ArgumentNullException(nameof(permissionsDialogService));
        }

        private bool IsPermissionDeniedEver<T>()
            where T : BasePermission
        {
            return Preferences.Get(GetPermissionDeniedEverKey<T>(), false);
        }

        private void SetPermissionDenied<T>(bool value)
            where T : BasePermission
        {
            Preferences.Set(GetPermissionDeniedEverKey<T>(), value);
        }

        private string GetPermissionDeniedEverKey<T>()
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

            if (permissionStatus == PermissionStatus.Denied
                && IsPermissionDeniedEver<T>()
                && !Xamarin.Essentials.Permissions.ShouldShowRationale<T>())
            {
                await OpenSettingsWithConfirmationAsync<T>().ConfigureAwait(false);
                return PermissionStatus.Denied;
            }

            var confirmationResult = await _permissionsDialogService.ConfirmPermissionAsync<T>().ConfigureAwait(false);
            if (confirmationResult)
            {
                permissionStatus = await _permissionsService.RequestPermissionsAsync<T>().ConfigureAwait(false);
                SetPermissionDenied<T>(permissionStatus == PermissionStatus.Denied);
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
