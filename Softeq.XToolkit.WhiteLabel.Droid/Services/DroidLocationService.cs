// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Softeq.XToolkit.Common.Weak;
using Softeq.XToolkit.WhiteLabel.Location;

namespace Softeq.XToolkit.WhiteLabel.Droid.Services
{
    /// <summary>
    ///     Android platform-specific implementation of <see cref="ILocationService"/> interface.
    /// </summary>
    public class DroidLocationService : ILocationService
    {
        private readonly Lazy<LocationManager?> _locationManagerLazy;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DroidLocationService"/> class.
        /// </summary>
        public DroidLocationService()
        {
            _locationManagerLazy = new Lazy<LocationManager?>(() =>
                Application.Context.GetSystemService(Context.LocationService) as LocationManager);
        }

        /// <summary>
        ///     Gets the name of the provider that is used to get the location.
        /// </summary>
        protected virtual string ProviderName => LocationManager.NetworkProvider;

        /// <inheritdoc />
        public bool IsLocationServiceEnabled => _locationManagerLazy.Value?.IsProviderEnabled(ProviderName) ?? false;

        /// <inheritdoc />
        /// <remarks>
        ///     Uses Network provider to determine a location based on nearby cell tower and WiFi access points.
        /// </remarks>
        public Task<LocationModel?> GetCurrentLocation()
        {
            if (_locationManagerLazy.Value == null)
            {
                return Task.FromResult<LocationModel?>(null);
            }

            var tcs = new TaskCompletionSource<LocationModel?>();
            var listener = new LocationListener(_locationManagerLazy.Value, tcs);
            var looper = Looper.MyLooper() ?? Looper.MainLooper;

            _locationManagerLazy.Value.RequestLocationUpdates(ProviderName, 0, 0, listener, looper);

            return tcs.Task;
        }

        private class LocationListener : Java.Lang.Object, ILocationListener
        {
            private readonly WeakReferenceEx<LocationManager> _locationManagerRef;

            private TaskCompletionSource<LocationModel?>? _tcs;

            public LocationListener(
                LocationManager locationManager,
                TaskCompletionSource<LocationModel?> tcs)
            {
                _locationManagerRef = WeakReferenceEx.Create(locationManager);
                _tcs = tcs;
            }

            public void OnLocationChanged(Android.Locations.Location? location)
            {
                if (_tcs == null)
                {
                    return;
                }

                _tcs.TrySetResult(location == null
                    ? null
                    : new LocationModel
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude
                    });

                _locationManagerRef.Target?.RemoveUpdates(this);
                _tcs = null;
            }

            public void OnProviderDisabled(string? provider)
            {
            }

            public void OnProviderEnabled(string? provider)
            {
            }

            public void OnStatusChanged(string? provider, [GeneratedEnum] Availability status, Bundle? extras)
            {
            }
        }
    }
}
