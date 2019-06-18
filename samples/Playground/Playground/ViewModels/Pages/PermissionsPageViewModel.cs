using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Playground.ViewModels.Pages
{
    public class PermissionsPageViewModel : ViewModelBase
    {
        private readonly IPermissionsManager _permissionsManager;

        public bool _photosGranted;
        public bool _cameraGranted;
        public bool _storageGranted;
        public bool _locationInUseGranted;
        public bool _locationAlwaysGranted;

        public PermissionsPageViewModel(IPermissionsManager permissionsManager)
        {
            _permissionsManager = permissionsManager;

            RequestCalendarPermissionCommand = new AsyncCommand<Permission>(OnRequestPermission);
        }

        public bool PhotosGranted
        {
            get => _photosGranted;
            set
            {
                _photosGranted = value;
                RaisePropertyChanged(nameof(PhotosGranted));
            }
        }

        public bool CameraGranted
        {
            get => _cameraGranted;
            set
            {
                _cameraGranted = value;
                RaisePropertyChanged(nameof(CameraGranted));
            }
        }

        public bool StorageGranted
        {
            get => _storageGranted;
            set
            {
                _storageGranted = value;
                RaisePropertyChanged(nameof(StorageGranted));
            }
        }

        public bool LocationInUseGranted
        {
            get => _locationInUseGranted;
            set
            {
                _locationInUseGranted = value;
                RaisePropertyChanged(nameof(LocationInUseGranted));
            }
        }

        public bool LocationAlwaysGranted
        {
            get => _locationAlwaysGranted;
            set
            {
                _locationAlwaysGranted = value;
                RaisePropertyChanged(nameof(LocationAlwaysGranted));
            }
        }

        public ICommand<Permission> RequestCalendarPermissionCommand;

        public override void OnInitialize()
        {
            base.OnInitialize();
            ReloadData();
        }

        private async Task CheckPermission(Permission permission)
        {
            var status = await _permissionsManager.CheckAsync(permission).ConfigureAwait(false);
            UpdatePermissionStatus(permission, status);
        }

        private async Task OnRequestPermission(Permission permission)
        {
            var status = await _permissionsManager.CheckWithRequestAsync(permission).ConfigureAwait(false);
            ReloadData();
        }

        private void ReloadData()
        {
            foreach (Permission item in Enum.GetValues(typeof(Permission)))
            {
                CheckPermission(item).SafeTaskWrapper();
            }
        }

        private void UpdatePermissionStatus(Permission permission, PermissionStatus status)
        {
            Execute.OnUIThread(() =>
            {
                bool granted = status == PermissionStatus.Granted;
                switch (permission)
                {
                    case Permission.Photos:
                        PhotosGranted = granted;
                        break;
                    case Permission.Camera:
                        CameraGranted = granted;
                        break;
                    case Permission.Storage:
                        StorageGranted = granted;
                        break;
                    case Permission.LocationAlways:
                        LocationAlwaysGranted = granted;
                        break;
                    case Permission.LocationInUse:
                        LocationInUseGranted = granted;
                        break;
                }
            });
        }
    }
}
