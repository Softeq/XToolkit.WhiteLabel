// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Softeq.XToolkit.Permissions.iOS
{
    /// <inheritdoc cref="IPermissionsManager" />
    public class PermissionsManager : IPermissionsManager
    {
        private readonly ISettings _internalSettings;

        private readonly string _isNotificationsPermissionRequestedKey =
            $"{nameof(PermissionsManager)}_{nameof(IsNotificationsPermissionRequested)}";

        private readonly IPermissionsService _permissionsService;

        private IPermissionsDialogService _permissionsDialogService;

        public PermissionsManager(
            IPermissionsService permissionsService)
        {
            _permissionsService = permissionsService;
            _internalSettings = CrossSettings.Current;
            _permissionsDialogService = new DefaultPermissionsDialogService();
        }

        private bool IsNotificationsPermissionRequested
        {
            get => _internalSettings.GetValueOrDefault(_isNotificationsPermissionRequestedKey, false);
            set => _internalSettings.AddOrUpdateValue(_isNotificationsPermissionRequestedKey, value);
        }

        /// <inheritdoc />
        public Task<PermissionStatus> CheckWithRequestAsync<T>()
            where T : BasePermission, new()
        {
            return typeof(T) == typeof(NotificationsPermission)
                ? NotificationsCheckWithRequestAsync()
                : CommonCheckWithRequestAsync<T>();
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

        private void OpenSettings()
        {
            _permissionsService.OpenSettings();
        }

        private async Task<PermissionStatus> NotificationsCheckWithRequestAsync()
        {
            if (!IsNotificationsPermissionRequested)
            {
                var isConfirmed = await _permissionsDialogService.ConfirmPermissionAsync<NotificationsPermission>()
                    .ConfigureAwait(false);
                if (!isConfirmed)
                {
                    return PermissionStatus.Denied;
                }
            }

            var permissionStatus =
                await _permissionsService.RequestPermissionsAsync<NotificationsPermission>().ConfigureAwait(false);

            if (IsNotificationsPermissionRequested && permissionStatus != PermissionStatus.Granted)
            {
                permissionStatus = await OpenSettingsWithConfirmationAsync<NotificationsPermission>().ConfigureAwait(false);
            }

            IsNotificationsPermissionRequested = true;

            return permissionStatus;
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
