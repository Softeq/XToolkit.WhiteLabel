// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Networking;

namespace Softeq.XToolkit.Connectivity
{
    /// <summary>
    ///    MAUI.Essentials cross-platform implementation of <see cref="IConnectivityService"/>.
    /// </summary>
    public class EssentialsConnectivityService : IConnectivityService, IDisposable
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="EssentialsConnectivityService"/> class.
        /// </summary>
        /// <param name="connectivity">
        ///     Custom instance of <see cref="T:Microsoft.Maui.Networking.IConnectivity"/>
        ///     or you can use <see cref="Default"/> static method.
        /// </param>
        public EssentialsConnectivityService(IConnectivity connectivity)
        {
            Connectivity = connectivity;
            Connectivity.ConnectivityChanged += CurrentConnectivityChanged;
        }

        ~EssentialsConnectivityService()
        {
            Dispose(false);
        }

        /// <inheritdoc />
        public event EventHandler<ConnectivityChangedEventArgs>? ConnectivityChanged;

        protected IConnectivity Connectivity { get; private set; }

        /// <inheritdoc />
        public virtual bool IsConnected
        {
            get
            {
                var profiles = Connectivity.ConnectionProfiles;
                var access = Connectivity.NetworkAccess;

                var hasAnyConnection = profiles.Any();
                var hasInternet = access == NetworkAccess.Internet;

                return hasAnyConnection && hasInternet;
            }
        }

        /// <inheritdoc />
        public IEnumerable<ConnectionProfile> ConnectionProfiles => Connectivity.ConnectionProfiles;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EssentialsConnectivityService"/> class
        ///     with a default instance of <see cref="T:Microsoft.Maui.Networking.IConnectivity"/>.
        /// </summary>
        /// <returns><see cref="EssentialsConnectivityService"/> instance.</returns>
        public static EssentialsConnectivityService Default() => new(Microsoft.Maui.Networking.Connectivity.Current);

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Releases the unmanaged and optionally the managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to dispose managed state.</param>
        /// <seealso cref="Dispose"/>
        /// <seealso cref="T:System.IDisposable"/>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Connectivity.ConnectivityChanged -= CurrentConnectivityChanged;
            }
        }

        protected void CurrentConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
        {
            ConnectivityChanged?.Invoke(sender, e);
        }
    }
}
