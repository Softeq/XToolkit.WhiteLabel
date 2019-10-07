// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Plugin.Permissions;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.Components
{
    public class PermissionsPageViewModel : ViewModelBase
    {
        public PermissionsPageViewModel(IPermissionsManager permissionsManager)
        {
            Photos = new PermissionViewModel<PhotosPermission>(permissionsManager);
            Camera = new PermissionViewModel<CameraPermission>(permissionsManager);
            Storage = new PermissionViewModel<StoragePermission>(permissionsManager);
            Location = new PermissionViewModel<LocationPermission>(permissionsManager);
            LocationInUse = new PermissionViewModel<LocationWhenInUsePermission>(permissionsManager);
            LocationAlways = new PermissionViewModel<LocationAlwaysPermission>(permissionsManager);
        }

        public PermissionViewModel<PhotosPermission> Photos { get; }

        public PermissionViewModel<CameraPermission> Camera { get; }

        public PermissionViewModel<StoragePermission> Storage { get; }

        public PermissionViewModel<LocationPermission> Location { get; }

        public PermissionViewModel<LocationWhenInUsePermission> LocationInUse { get; }

        public PermissionViewModel<LocationAlwaysPermission> LocationAlways { get; }

        public override void OnAppearing()
        {
            base.OnAppearing();

            CheckAll().SafeTaskWrapper();
        }

        private async Task CheckAll()
        {
            await Photos.CheckStatus();

            await Camera.CheckStatus();

            await Storage.CheckStatus();

            await Location.CheckStatus();

            await LocationInUse.CheckStatus();

            await LocationAlways.CheckStatus();
        }
    }
}
