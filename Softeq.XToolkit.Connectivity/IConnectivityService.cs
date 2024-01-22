// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Microsoft.Maui.Networking;

namespace Softeq.XToolkit.Connectivity
{
    /// <summary>
    ///     Interface for Connectivity Service.
    /// </summary>
    public interface IConnectivityService
    {
        /// <summary>
        ///     Event handler when connection state changes.
        /// </summary>
        event EventHandler<ConnectivityChangedEventArgs> ConnectivityChanged;

        /// <summary>
        ///     Gets a value indicating whether there is an active internet connection.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        ///     Gets the active connectivity types for the device.
        /// </summary>
        IEnumerable<ConnectionProfile> ConnectionProfiles { get; }
    }
}
