// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.WeakSubscription;
using Plugin.Connectivity.Abstractions;
using System.Collections.Generic;

namespace Softeq.XToolkit.Connectivity
{
    public class ConnectivityService : IConnectivityService
    {
        public virtual event ConnectivityChangedEventHandler ConnectivityChanged;
        public virtual event ConnectivityTypeChangedEventHandler ConnectivityTypeChanged;

        public ConnectivityService()
        {
            Plugin.Connectivity.CrossConnectivity.Current.ConnectivityChanged += CurrentConnectivityChanged;
            Plugin.Connectivity.CrossConnectivity.Current.ConnectivityTypeChanged += CurrentConnectivityTypeChanged;
        }

        public virtual bool IsConnected => Plugin.Connectivity.CrossConnectivity.Current.IsConnected;

        public bool IsSupported => Plugin.Connectivity.CrossConnectivity.IsSupported;

        public IEnumerable<ConnectionType> ConnectionTypes => Plugin.Connectivity.CrossConnectivity.Current.ConnectionTypes;

        private void CurrentConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            ConnectivityChanged?.Invoke(sender, e);
        }

        private void CurrentConnectivityTypeChanged(object sender, ConnectivityTypeChangedEventArgs e)
        {
            ConnectivityTypeChanged?.Invoke(sender, e);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ConnectivityService()
        {
            Dispose(false);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                Plugin.Connectivity.CrossConnectivity.Current.ConnectivityChanged -= CurrentConnectivityChanged;
                Plugin.Connectivity.CrossConnectivity.Current.ConnectivityTypeChanged -= CurrentConnectivityTypeChanged;
            }
        }
    }
}
