// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using CoreLocation;
using Softeq.XToolkit.WhiteLabel.Location;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    /// <summary>
    ///     iOS platform-specific implementation of <see cref="ILocationService"/> interface.
    /// </summary>
    public class IosLocationService : ILocationService
    {
        private readonly Lazy<CLLocationManager> _locationManagerLazy =
            new Lazy<CLLocationManager>(() => new CLLocationManager());

        /// <inheritdoc />
        public bool IsLocationServiceEnabled => CLLocationManager.LocationServicesEnabled;

        /// <inheritdoc />
        public Task<LocationModel?> GetCurrentLocation()
        {
            LocationModel? result = default;

            var location = _locationManagerLazy.Value.Location;
            if (location != null)
            {
                result = new LocationModel
                {
                    Latitude = location.Coordinate.Latitude,
                    Longitude = location.Coordinate.Longitude
                };
            }

            return Task.FromResult(result);
        }
    }
}
