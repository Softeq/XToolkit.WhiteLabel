// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CoreFoundation;
using Microsoft.Maui.Networking;
using Network;

namespace Softeq.XToolkit.Connectivity.iOS
{
    /// <summary>
    ///     iOS implementation of <see cref="IConnectivityService"/>.
    ///     Uses iOS 12+ Network framework: https://developer.apple.com/documentation/network?language=objc
    ///     MAUI.Essentials issue: https://github.com/dotnet/maui/issues/2574 .
    /// </summary>
    public class IosConnectivityService : IConnectivityService, IDisposable
    {
        private readonly ReaderWriterLockSlim _statusesLock;
        private readonly Dictionary<NWInterfaceType, bool> _connectionStatuses;
        private readonly IList<NWPathMonitor> _monitors;

        /// <summary>
        ///     Initializes a new instance of the <see cref="IosConnectivityService"/> class.
        /// </summary>
        public IosConnectivityService()
        {
            _statusesLock = new ReaderWriterLockSlim();
            _connectionStatuses = new Dictionary<NWInterfaceType, bool>();
            _monitors = new List<NWPathMonitor>
            {
                RegisterMonitor(NWInterfaceType.Cellular),
                RegisterMonitor(NWInterfaceType.Wifi),
                RegisterMonitor(NWInterfaceType.Wired),
                RegisterMonitor(NWInterfaceType.Loopback),
                RegisterMonitor(NWInterfaceType.Other)
            };
        }

        ~IosConnectivityService()
        {
            Dispose(false);
        }

        /// <inheritdoc />
        public event EventHandler<ConnectivityChangedEventArgs>? ConnectivityChanged;

        /// <inheritdoc />
        public bool IsConnected
        {
            get
            {
                _statusesLock.EnterReadLock();
                try
                {
                    return CheckConnectivity(_connectionStatuses);
                }
                finally
                {
                    _statusesLock.ExitReadLock();
                }
            }
        }

        /// <inheritdoc />
        public IEnumerable<ConnectionProfile> ConnectionProfiles
        {
            get
            {
                _statusesLock.EnterReadLock();
                try
                {
                    var statuses = _connectionStatuses.Where(x => x.Value).Select(x => x.Key).ToList();
                    return FilterConnectionTypes(statuses);
                }
                finally
                {
                    _statusesLock.ExitReadLock();
                }
            }
        }

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
                foreach (var monitor in _monitors)
                {
                    monitor.Cancel();
                    monitor.Dispose();
                }

                _monitors.Clear();
            }
        }

        protected virtual bool CheckConnectivity(IReadOnlyDictionary<NWInterfaceType, bool> connectionStatuses)
        {
            return connectionStatuses
                .Where(x => x.Key == NWInterfaceType.Wifi || x.Key == NWInterfaceType.Cellular)
                .Any(x => x.Value);
        }

        protected virtual IEnumerable<ConnectionProfile> FilterConnectionTypes(IList<NWInterfaceType> activeNetworkTypes)
        {
            if (activeNetworkTypes.Contains(NWInterfaceType.Other) && activeNetworkTypes.Count > 1)
            {
                activeNetworkTypes.Remove(NWInterfaceType.Other);
            }

            return activeNetworkTypes.Select(Convert);
        }

        protected virtual ConnectionProfile Convert(NWInterfaceType networkType)
        {
            return networkType switch
            {
                NWInterfaceType.Cellular => ConnectionProfile.Cellular,
                NWInterfaceType.Wifi => ConnectionProfile.WiFi,
                NWInterfaceType.Wired => ConnectionProfile.Ethernet,
                NWInterfaceType.Loopback => ConnectionProfile.Unknown,
                _ => ConnectionProfile.Unknown
            };
        }

        private NWPathMonitor RegisterMonitor(NWInterfaceType type)
        {
            _statusesLock.EnterWriteLock();
            try
            {
                _connectionStatuses.TryAdd(type, false);
            }
            finally
            {
                _statusesLock.ExitWriteLock();
            }

            var monitor = CreateMonitor(type, nWPath => HandleUpdateSnapshot(nWPath, type));
            monitor.Start();
            return monitor;
        }

        private NWPathMonitor CreateMonitor(NWInterfaceType type, Action<NWPath> action)
        {
            var monitor = new NWPathMonitor(type);
            monitor.SetQueue(DispatchQueue.GetGlobalQueue(DispatchQueuePriority.Background));
            monitor.SnapshotHandler = action;
            return monitor;
        }

        private void HandleUpdateSnapshot(NWPath nWPath, NWInterfaceType type)
        {
            var isConnectedOld = IsConnected;
            var connectionTypesOld = ConnectionProfiles;

            _statusesLock.EnterWriteLock();
            try
            {
                _connectionStatuses[type] = nWPath.Status == NWPathStatus.Satisfied;
            }
            finally
            {
                _statusesLock.ExitWriteLock();
            }

            var isConnectedNew = IsConnected;
            var connectionTypesNew = ConnectionProfiles;

            if (isConnectedOld != isConnectedNew ||
                !connectionTypesNew.SequenceEqual(connectionTypesOld))
            {
                var access = isConnectedNew ? NetworkAccess.Internet : NetworkAccess.None;
                ConnectivityChanged?.Invoke(this, new ConnectivityChangedEventArgs(access, ConnectionProfiles));
            }
        }
    }
}
