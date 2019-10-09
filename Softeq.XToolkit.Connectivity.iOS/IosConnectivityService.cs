// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Network;
using Plugin.Connectivity.Abstractions;
using UIKit;

namespace Softeq.XToolkit.Connectivity.iOS
{
    public class IosConnectivityService : IConnectivityService
    {
        private readonly NWPathMonitor _ceccularMonitor;
        private readonly NWPathMonitor _wifiMonitor;
        private readonly NWPathMonitor _wiredMonitor;
        private readonly NWPathMonitor _loopbackMonitor;
        private readonly NWPathMonitor _otherMonitor;

        private readonly Dictionary<NWInterfaceType, bool> _connectionStatuses;

        public event ConnectivityChangedEventHandler ConnectivityChanged;
        public event ConnectivityTypeChangedEventHandler ConnectivityTypeChanged;

        public IosConnectivityService()
        {
            _connectionStatuses = new Dictionary<NWInterfaceType, bool>();

            _ceccularMonitor = CreateMonitor(NWInterfaceType.Cellular, UpdateCeccularSnapshot);
            _wifiMonitor = CreateMonitor(NWInterfaceType.Wifi, UpdateWiFiSnapshot);
            _wiredMonitor = CreateMonitor(NWInterfaceType.Wired, UpdateWiredSnapshot);
            _loopbackMonitor = CreateMonitor(NWInterfaceType.Loopback, UpdateLoopbackSnapshot);
            _otherMonitor = CreateMonitor(NWInterfaceType.Other, UpdateOtherSnapshot);
        }

        public bool IsConnected => _connectionStatuses.Values.Any(x => x);

        public bool IsSupported => true;

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
            bool isConnectedOld = IsConnected;
            var connectionTypesOld = _connectionStatuses.Where(x => x.Value).Select(x => x.Key).ToList();

            _connectionStatuses[type] = nWPath.Status == NWPathStatus.Satisfied;

            if (isConnectedOld != IsConnected)
            {
                ConnectivityChanged?.Invoke(this, new ConnectivityChangedEventArgs
                {
                    IsConnected = IsConnected
                });
            }

            var connectionTypes = _connectionStatuses.Where(x => x.Value).Select(x => x.Key);

            if (!connectionTypes.SequenceEqual(connectionTypesOld))
            {
                ConnectivityTypeChanged?.Invoke(this, new ConnectivityTypeChangedEventArgs
                {
                    IsConnected = IsConnected,
                    ConnectionTypes = ConvertTypes(connectionTypes)
                });
            }
        }

        private IEnumerable<ConnectionType> ConvertTypes(IEnumerable<NWInterfaceType> nWInterfaceTypes)
        {
            var types = nWInterfaceTypes
                .Select(x =>
                {
                    switch (x)
                    {
                        case NWInterfaceType.Cellular:
                            return ConnectionType.Cellular;
                        case NWInterfaceType.Wifi:
                            return ConnectionType.WiFi;
                        default:
                            return ConnectionType.Other;
                    }
                }).ToList();

            if(types.Contains(ConnectionType.Other) && types.Count > 1)
            {
                types.Remove(ConnectionType.Other);
            }

            return types;
        }

        private NWPathMonitor CreateMonitor(NWInterfaceType type, Action<NWPath> action)
        {
            _connectionStatuses.Add(type, false);

            var monitor = new NWPathMonitor(type);
            monitor.SetQueue(CoreFoundation.DispatchQueue.MainQueue);
            monitor.SetUpdatedSnapshotHandler(action);
            monitor.Start();
            return monitor;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~IosConnectivityService()
        {
            Dispose(false);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                _ceccularMonitor.Dispose();
                _wifiMonitor.Dispose();
                _wiredMonitor.Dispose();
                _loopbackMonitor.Dispose();
                _otherMonitor.Dispose();
            }
        }
    }
}
