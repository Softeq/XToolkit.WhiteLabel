// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace Softeq.XToolkit.Connectivity
{
    public class ConnectivityService : IConnectivityService
    {
        public virtual event EventHandler<ConnectivityChangedEventArgs>? ConnectivityChanged;
        public virtual event EventHandler<ConnectivityTypeChangedEventArgs>? ConnectivityTypeChanged;

        public ConnectivityService()
        {
            CrossConnectivity.Current.ConnectivityChanged += CurrentConnectivityChanged;
            CrossConnectivity.Current.ConnectivityTypeChanged += CurrentConnectivityTypeChanged;
        }

        public virtual bool IsConnected => CrossConnectivity.Current.IsConnected;

        public bool IsSupported => CrossConnectivity.IsSupported;

        public IEnumerable<ConnectionType> ConnectionTypes => CrossConnectivity.Current.ConnectionTypes;

        public void Dispose()
        {
            CrossConnectivity.Current.ConnectivityChanged -= CurrentConnectivityChanged;
            CrossConnectivity.Current.ConnectivityTypeChanged -= CurrentConnectivityTypeChanged;
        }

        private void CurrentConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            ConnectivityChanged?.Invoke(sender, e);
        }

        private void CurrentConnectivityTypeChanged(object sender, ConnectivityTypeChangedEventArgs e)
        {
            ConnectivityTypeChanged?.Invoke(sender, e);
        }
    }
}
