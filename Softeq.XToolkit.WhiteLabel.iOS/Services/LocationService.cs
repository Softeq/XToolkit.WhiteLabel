// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using CoreLocation;
using Softeq.XToolkit.WhiteLabel.Location;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    public class LocationService : ILocationService
    {
        private readonly Lazy<CLLocationManager> _locationManagerLazy =
            new Lazy<CLLocationManager>(() => new CLLocationManager());

        public bool IsLocationServiceEnabled => CLLocationManager.LocationServicesEnabled;

        public Task<LocationModel> GetCurrentLocation()
        {
            var location = _locationManagerLazy.Value.Location;
            if (location == null)
            {
                return Task.FromResult(default(LocationModel));
            }

            return Task.FromResult(new LocationModel
            {
                Latitude = location.Coordinate.Latitude,
                Longitude = location.Coordinate.Longitude
            });
        }
    }
}