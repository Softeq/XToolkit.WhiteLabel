// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Plugin.Connectivity.Abstractions;

namespace Softeq.XToolkit.Connectivity
{
    public interface IConnectivityService : IDisposable
    {
        event ConnectivityChangedEventHandler ConnectivityChanged;

        event ConnectivityTypeChangedEventHandler ConnectivityTypeChanged;

        bool IsConnected { get; }

        bool IsSupported { get; }

        IEnumerable<ConnectionType> ConnectionTypes { get; }
    }
}
