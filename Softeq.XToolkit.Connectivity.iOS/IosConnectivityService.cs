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
        private readonly NWPathMonitor _cellularMonitor;
        private readonly NWPathMonitor _wifiMonitor;
        private readonly NWPathMonitor _wiredMonitor;
        private readonly NWPathMonitor _loopbackMonitor;
        private readonly NWPathMonitor _otherMonitor;

        private readonly Dictionary<NWInterfaceType, bool> _connectionStatuses;

        public event ConnectivityChangedEventHandler? ConnectivityChanged;
        public event ConnectivityTypeChangedEventHandler? ConnectivityTypeChanged;

        public IosConnectivityService()
        {
            _connectionStatuses = new Dictionary<NWInterfaceType, bool>();

            _cellularMonitor = CreateMonitor(NWInterfaceType.Cellular, UpdateCeccularSnapshot);
            _wifiMonitor = CreateMonitor(NWInterfaceType.Wifi, UpdateWiFiSnapshot);
            _wiredMonitor = CreateMonitor(NWInterfaceType.Wired, UpdateWiredSnapshot);
            _loopbackMonitor = CreateMonitor(NWInterfaceType.Loopback, UpdateLoopbackSnapshot);
            _otherMonitor = CreateMonitor(NWInterfaceType.Other, UpdateOtherSnapshot);
        }

        ~IosConnectivityService()
        {
            Dispose(false);
        }

        public bool IsConnected => _connectionStatuses.Values.Any(x => x);

        public bool IsSupported => true;

        public IEnumerable<ConnectionType> ConnectionTypes
        {
            get
            {
                var statuses = _connectionStatuses.Where(x => x.Value).Select(x => x.Key).ToList();

                if (statuses.Contains(NWInterfaceType.Other) && statuses.Count > 1)
                {
                    statuses.Remove(NWInterfaceType.Other);
                }

                return ConvertTypes(statuses);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cellularMonitor.Cancel();
                _wifiMonitor.Cancel();
                _wiredMonitor.Cancel();
                _loopbackMonitor.Cancel();
                _otherMonitor.Cancel();

                _cellularMonitor.Dispose();
                _wifiMonitor.Dispose();
                _wiredMonitor.Dispose();
                _loopbackMonitor.Dispose();
                _otherMonitor.Dispose();
            }
        }

        private void UpdateWiredSnapshot(NWPath nWPath)
        {
            HandleUpdateSnapshot(nWPath, NWInterfaceType.Wired);
        }

        private void UpdateWiFiSnapshot(NWPath nWPath)
        {
            HandleUpdateSnapshot(nWPath, NWInterfaceType.Wifi);
        }

        private void UpdateCeccularSnapshot(NWPath nWPath)
        {
            HandleUpdateSnapshot(nWPath, NWInterfaceType.Cellular);
        }

        private void UpdateLoopbackSnapshot(NWPath nWPath)
        {
            HandleUpdateSnapshot(nWPath, NWInterfaceType.Loopback);
        }

        private void UpdateOtherSnapshot(NWPath nWPath)
        {
            HandleUpdateSnapshot(nWPath, NWInterfaceType.Other);
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

        private IEnumerable<ConnectionType> ConvertTypes(IEnumerable<NWInterfaceType> networkInterfaceTypes)
        {
            return networkInterfaceTypes
                .Select(x =>
                {
                    return x switch
                    {
                        NWInterfaceType.Cellular => ConnectionType.Cellular,
                        NWInterfaceType.Wifi => ConnectionType.WiFi,
                        _ => ConnectionType.Other
                    };
                });
        }

        private NWPathMonitor CreateMonitor(NWInterfaceType type, Action<NWPath> action)
        {
            _connectionStatuses.Add(type, false);

            var monitor = new NWPathMonitor(type);
            monitor.SetQueue(DispatchQueue.GetGlobalQueue(DispatchQueuePriority.Background));
            monitor.SnapshotHandler = action;
            monitor.Start();
            return monitor;
        }
    }
}
