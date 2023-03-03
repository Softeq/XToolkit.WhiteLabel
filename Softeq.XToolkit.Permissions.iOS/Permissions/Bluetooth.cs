// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using CoreBluetooth;
using BasePlatformPermission = Microsoft.Maui.ApplicationModel.Permissions.BasePlatformPermission;
using EssentialsPermissionStatus = Microsoft.Maui.ApplicationModel.PermissionStatus;

namespace Softeq.XToolkit.Permissions.iOS.Permissions
{
    public class Bluetooth : BasePlatformPermission
    {
        protected override Func<IEnumerable<string>> RequiredInfoPlistKeys =>
            () => new string[] { "NSBluetoothAlwaysUsageDescription" };

        public override Task<EssentialsPermissionStatus> CheckStatusAsync()
        {
            EnsureDeclared();

            return Task.FromResult(ParseAuthorization(CBManager.Authorization));
        }

        public override async Task<EssentialsPermissionStatus> RequestAsync()
        {
            EnsureDeclared();

            var status = await CheckStatusAsync();

            if (status == EssentialsPermissionStatus.Granted)
            {
                return status;
            }

            if (CBManager.Authorization == CBManagerAuthorization.NotDetermined)
            {
                var centralManagerDelegate = new CentralManagerDelegate();
                await centralManagerDelegate.RequestAccessAsync();
            }

            return ParseAuthorization(CBManager.Authorization);
        }

        private static EssentialsPermissionStatus ParseAuthorization(CBManagerAuthorization managerAuthorization)
        {
            return managerAuthorization switch
            {
                CBManagerAuthorization.NotDetermined => EssentialsPermissionStatus.Unknown,
                CBManagerAuthorization.AllowedAlways => EssentialsPermissionStatus.Granted,
                CBManagerAuthorization.Restricted => EssentialsPermissionStatus.Granted,
                CBManagerAuthorization.Denied => EssentialsPermissionStatus.Denied,
                _ => EssentialsPermissionStatus.Unknown
            };
        }

        private class CentralManagerDelegate : CBCentralManagerDelegate
        {
            private readonly CBCentralManager _centralManager;

            private TaskCompletionSource<CBManagerState> _statusRequest =
                new TaskCompletionSource<CBManagerState>();

            public CentralManagerDelegate()
            {
                _centralManager = new CBCentralManager(this, null);
            }

            public async Task<CBManagerState> RequestAccessAsync()
            {
                var state = await _statusRequest.Task;
                return state;
            }

            public override void UpdatedState(CBCentralManager centralManager)
            {
                if (_centralManager.State != CBManagerState.Unknown)
                {
                    _statusRequest.TrySetResult(_centralManager.State);
                }
            }
        }
    }
}