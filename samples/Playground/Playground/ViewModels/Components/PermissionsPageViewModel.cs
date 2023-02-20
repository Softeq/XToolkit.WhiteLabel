// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using BluetoothPermission = Softeq.XToolkit.Permissions.Bluetooth;
using CameraPermission = Xamarin.Essentials.Permissions.Camera;
using LocationAlwaysPermission = Xamarin.Essentials.Permissions.LocationAlways;
using LocationWhenInUsePermission = Xamarin.Essentials.Permissions.LocationWhenInUse;
using NotificationsPermission = Softeq.XToolkit.Permissions.Notifications;
using PhotosPermission = Xamarin.Essentials.Permissions.Photos;
using StoragePermission = Xamarin.Essentials.Permissions.StorageWrite;

namespace Playground.ViewModels.Components
{
    public class PermissionsPageViewModel : ViewModelBase
    {
        public PermissionsPageViewModel(IPermissionsManager permissionsManager)
        {
            Photos = new PermissionViewModel<PhotosPermission>(permissionsManager);
            Camera = new PermissionViewModel<CameraPermission>(permissionsManager);
            Storage = new PermissionViewModel<StoragePermission>(permissionsManager);
            LocationInUse = new PermissionViewModel<LocationWhenInUsePermission>(permissionsManager);
            LocationAlways = new PermissionViewModel<LocationAlwaysPermission>(permissionsManager);
            Notifications = new PermissionViewModel<NotificationsPermission>(permissionsManager);
            Bluetooth = new PermissionViewModel<BluetoothPermission>(permissionsManager);
        }

        public PermissionViewModel<PhotosPermission> Photos { get; }

        public PermissionViewModel<CameraPermission> Camera { get; }

        public PermissionViewModel<StoragePermission> Storage { get; }

        public PermissionViewModel<LocationWhenInUsePermission> LocationInUse { get; }

        public PermissionViewModel<LocationAlwaysPermission> LocationAlways { get; }

        public PermissionViewModel<NotificationsPermission> Notifications { get; }

        public PermissionViewModel<BluetoothPermission> Bluetooth { get; }

        public override void OnAppearing()
        {
            base.OnAppearing();

            CheckAll().FireAndForget();
        }

        private async Task CheckAll()
        {
            await Photos.CheckStatus();

            await Camera.CheckStatus();

            await Storage.CheckStatus();

            await LocationInUse.CheckStatus();

            await LocationAlways.CheckStatus();

            await Notifications.CheckStatus();

            await Bluetooth.CheckStatus();
        }
    }
}