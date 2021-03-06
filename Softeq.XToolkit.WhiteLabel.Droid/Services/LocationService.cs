﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Softeq.XToolkit.Common.Weak;
using Softeq.XToolkit.WhiteLabel.Droid.Providers;
using Softeq.XToolkit.WhiteLabel.Location;
using Object = Java.Lang.Object;

namespace Softeq.XToolkit.WhiteLabel.Droid.Services
{
    // TODO YP: Rework to Xamarin.Essentials
    public class LocationService : ILocationService
    {
        private readonly IContextProvider _contextProvider;
        private readonly Lazy<LocationManager> _locationManagerLazy;
        private readonly string _providerName = LocationManager.NetworkProvider;

        public LocationService(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
            _locationManagerLazy = new Lazy<LocationManager>(() =>
            {
                var currentActivity = _contextProvider.CurrentActivity;
                return (LocationManager) currentActivity.GetSystemService(Context.LocationService);
            });
        }

        public bool IsLocationServiceEnabled => _locationManagerLazy.Value.IsProviderEnabled(_providerName);

        public Task<LocationModel?> GetCurrentLocation()
        {
            var tcs = new TaskCompletionSource<LocationModel?>();
            _contextProvider.CurrentActivity.RunOnUiThread(() =>
                _locationManagerLazy.Value.RequestLocationUpdates(
                    _providerName,
                    0,
                    0,
                    new LocationListener(_locationManagerLazy.Value, tcs)));
            return tcs.Task;
        }

        private class LocationListener : Object, ILocationListener
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
                _locationManagerRef.Target.RemoveUpdates(this);
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
