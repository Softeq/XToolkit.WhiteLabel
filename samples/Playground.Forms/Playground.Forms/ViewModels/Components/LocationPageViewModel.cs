// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Softeq.XToolkit.WhiteLabel.Location;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Xamarin.Essentials;
using PermissionStatus = Softeq.XToolkit.Permissions.PermissionStatus;

namespace Playground.Forms.ViewModels.Components
{
    public class LocationPageViewModel : ViewModelBase
    {
        private readonly ILocationService _locationService;
        private readonly IPermissionsManager _permissionsManager;
        private readonly IDialogsService _dialogsService;

        private LocationModel? _location;

        public LocationPageViewModel(
            ILocationService locationService,
            IPermissionsManager permissionsManager,
            IDialogsService dialogsService)
        {
            _locationService = locationService;
            _permissionsManager = permissionsManager;
            _dialogsService = dialogsService;

            GetLocationCommand = new AsyncCommand(GetLocationAsync);
        }

        public LocationModel? Location
        {
            get => _location;
            private set => Set(ref _location, value);
        }

        public IAsyncCommand GetLocationCommand { get; }

        private async Task GetLocationAsync()
        {
            Location = null;

            var locationPermissionStatus = await _permissionsManager.CheckWithRequestAsync<Permissions.LocationWhenInUse>();
            if (locationPermissionStatus != PermissionStatus.Granted)
            {
                await _dialogsService.ShowDialogAsync(new AlertDialogConfig("Error", $"Location Permissions is required.", "Ok"));
                return;
            }

            if (!_locationService.IsLocationServiceEnabled)
            {
                await _dialogsService.ShowDialogAsync(new AlertDialogConfig("Error", "LocationService is disabled", "Ok"));
                return;
            }

            var location = await Task.Run(async () =>
            {
                // return await GetEssentialsLocationAsync();
                return await _locationService.GetCurrentLocation();
            });

            if (location == null)
            {
                await _dialogsService.ShowDialogAsync(new AlertDialogConfig("Error", "Unable to get current location.", "Ok"));
                return;
            }

            Location = location;
        }

        private async Task<LocationModel?> GetEssentialsLocationAsync()
        {
            try
            {
                var location = await Geolocation.GetLocationAsync().ConfigureAwait(false);
                return new LocationModel(location.Latitude, location.Longitude);
            }
            catch (Exception ex)
            {
                await _dialogsService.ShowDialogAsync(new AlertDialogConfig("Exception", ex.Message, "Ok"));
            }

            return null;
        }
    }
}
