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

            return Task.FromResult(ParseAuthorization(CoreBluetooth.CBManager.Authorization));
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
                var centralMamangerDelegate = new CentralManagerDelegate();
                await centralMamangerDelegate.StatusObserverable
                    .Select(x => x != CBCentralManagerState.Unknown)
                    .FirstAsync()
                    .ToTask();
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
            private readonly CBCentralManager _manager;
            private readonly Subject<CBCentralManagerState> _subject
                = new Subject<CBCentralManagerState>();

            public CentralManagerDelegate()
            {
                _manager = new CBCentralManager(this, null);
            }

            public IObservable<CBCentralManagerState> StatusObserverable => _subject;

            public override void UpdatedState(CBCentralManager central)
            {
                _subject.OnNext(_manager.State);
            }
        }
    }
}