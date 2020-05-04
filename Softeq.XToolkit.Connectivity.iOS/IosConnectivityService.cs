// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using CoreFoundation;
using Network;
using Plugin.Connectivity.Abstractions;

namespace Softeq.XToolkit.Connectivity.iOS
{
    public class IosConnectivityService : IConnectivityService
    {
        private readonly Dictionary<NWInterfaceType, bool> _connectionStatuses;
        private readonly IList<NWPathMonitor> _monitors;

        public event ConnectivityChangedEventHandler? ConnectivityChanged;
        public event ConnectivityTypeChangedEventHandler? ConnectivityTypeChanged;

        public IosConnectivityService()
        {
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

        public bool IsConnected => CheckConnectivity(_connectionStatuses);

        public bool IsSupported => true;

        public IEnumerable<ConnectionType> ConnectionTypes
        {
            get
            {
                var statuses = _connectionStatuses.Where(x => x.Value).Select(x => x.Key).ToList();
                return FilterConnectionTypes(statuses);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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

        protected virtual IEnumerable<ConnectionType> FilterConnectionTypes(IList<NWInterfaceType> activeNetworkTypes)
        {
            if (activeNetworkTypes.Contains(NWInterfaceType.Other) && activeNetworkTypes.Count > 1)
            {
                activeNetworkTypes.Remove(NWInterfaceType.Other);
            }

            return activeNetworkTypes.Select(Convert);
        }

        protected virtual ConnectionType Convert(NWInterfaceType networkType)
        {
            return networkType switch
            {
                NWInterfaceType.Cellular => ConnectionType.Cellular,
                NWInterfaceType.Wifi => ConnectionType.WiFi,
                _ => ConnectionType.Other
            };
        }

        private NWPathMonitor RegisterMonitor(NWInterfaceType type)
        {
            _connectionStatuses.TryAdd(type, false);

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
            var connectionTypesOld = ConnectionTypes;

            _connectionStatuses[type] = nWPath.Status == NWPathStatus.Satisfied;

            if (isConnectedOld != IsConnected)
            {
                ConnectivityChanged?.Invoke(this, new ConnectivityChangedEventArgs
                {
                    IsConnected = IsConnected
                });
            }

            if (!ConnectionTypes.SequenceEqual(connectionTypesOld))
            {
                ConnectivityTypeChanged?.Invoke(this, new ConnectivityTypeChangedEventArgs
                {
                    IsConnected = IsConnected,
                    ConnectionTypes = ConnectionTypes
                });
            }
        }
    }
}
