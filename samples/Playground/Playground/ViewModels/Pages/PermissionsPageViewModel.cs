// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Permissions;
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
        public bool _locationGranted;
        public bool _locationInUseGranted;
        public bool _locationAlwaysGranted;

        public PermissionsPageViewModel(IPermissionsManager permissionsManager)
        {
            _permissionsManager = permissionsManager;

            RequestCameraCommand = new AsyncCommand(OnRequestCameraPermission);

            RequestStorageCommand = new AsyncCommand(OnRequestStoragePermission);

            RequestLocationCommand = new AsyncCommand(OnRequestLocationPermission);

            RequestPhotosCommand = new AsyncCommand(OnRequestPhotosPermission);

            RequestLocationInUseCommand = new AsyncCommand(OnRequestLocationInUsePermission);

            RequestLocationAlwaysCommand = new AsyncCommand(OnRequestLocationAlwaysPermission);
        }

        public bool PhotosGranted
        {
            get => _photosGranted;
            set => Set(ref _photosGranted, value);
        }

        public bool CameraGranted
        {
            get => _cameraGranted;
            set => Set(ref _cameraGranted, value);
        }

        public bool StorageGranted
        {
            get => _storageGranted;
            set => Set(ref _storageGranted, value);
        }

        public bool LocationInUseGranted
        {
            get => _locationInUseGranted;
            set => Set(ref _locationInUseGranted, value);
        }

        public bool LocationGranted
        {
            get => _locationGranted;
            set => Set(ref _locationGranted, value);
        }

        public bool LocationAlwaysGranted
        {
            get => _locationAlwaysGranted;
            set => Set(ref _locationAlwaysGranted, value);
        }

        public ICommand RequestPhotosCommand { get; }

        public ICommand RequestCameraCommand { get; }

        public ICommand RequestStorageCommand { get; }

        public ICommand RequestLocationCommand { get; }

        public ICommand RequestLocationInUseCommand { get; }

        public ICommand RequestLocationAlwaysCommand { get; }

        public override void OnInitialize()
        {
            base.OnInitialize();
            ReloadData().SafeTaskWrapper();
        }

        private async Task OnRequestPhotosPermission()
        {
            await _permissionsManager.CheckWithRequestAsync<PhotosPermission>().ConfigureAwait(false);
            await ReloadData().ConfigureAwait(false);
        }

        private async Task OnRequestLocationInUsePermission()
        {
            await _permissionsManager.CheckWithRequestAsync<LocationWhenInUsePermission>().ConfigureAwait(false);
            await ReloadData().ConfigureAwait(false);
        }

        private async Task OnRequestLocationAlwaysPermission()
        {
            await _permissionsManager.CheckWithRequestAsync<LocationAlwaysPermission>().ConfigureAwait(false);
            await ReloadData().ConfigureAwait(false);
        }

        private async Task OnRequestCameraPermission()
        {
            await _permissionsManager.CheckWithRequestAsync<CameraPermission>().ConfigureAwait(false);
            await ReloadData().ConfigureAwait(false);
        }

        private async Task OnRequestStoragePermission()
        {
            await _permissionsManager.CheckWithRequestAsync<StoragePermission>().ConfigureAwait(false);
            await ReloadData().ConfigureAwait(false);
        }

        private async Task OnRequestLocationPermission()
        {
            await _permissionsManager.CheckWithRequestAsync<LocationPermission>().ConfigureAwait(false);
            await ReloadData().ConfigureAwait(false);
        }

        private async Task ReloadData()
        {
            var photosStatus = await _permissionsManager.CheckAsync<PhotosPermission>().ConfigureAwait(false);
            Execute.OnUIThread(() => PhotosGranted = photosStatus == PermissionStatus.Granted);

            var cameraStatus = await _permissionsManager.CheckAsync<CameraPermission>().ConfigureAwait(false);
            Execute.OnUIThread(() => CameraGranted = cameraStatus == PermissionStatus.Granted);

            var storageStatus = await _permissionsManager.CheckAsync<StoragePermission>().ConfigureAwait(false);
            Execute.OnUIThread(() => StorageGranted = storageStatus == PermissionStatus.Granted);

            var locationStatus = await _permissionsManager.CheckAsync<LocationPermission>().ConfigureAwait(false);
            Execute.OnUIThread(() => LocationGranted = locationStatus == PermissionStatus.Granted);

            var locationInUseStatus = await _permissionsManager.CheckAsync<LocationWhenInUsePermission>().ConfigureAwait(false);
            Execute.OnUIThread(() => LocationInUseGranted = locationInUseStatus == PermissionStatus.Granted);

            var locationAlwaysStatus = await _permissionsManager.CheckAsync<LocationAlwaysPermission>().ConfigureAwait(false);
            Execute.OnUIThread(() => LocationAlwaysGranted = locationAlwaysStatus == PermissionStatus.Granted);
        }
    }
}
