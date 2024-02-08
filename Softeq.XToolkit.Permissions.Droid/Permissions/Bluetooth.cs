// Developed by Softeq Development Corporation
// http://www.softeq.com

#if __ANDROID_31__
using System.Collections.Generic;
using Android;
using Android.OS;
#endif
using EssentialsPermissions = Microsoft.Maui.ApplicationModel.Permissions;

namespace Softeq.XToolkit.Permissions.Droid.Permissions
{
    public class Bluetooth : EssentialsPermissions.BasePlatformPermission
    {
        public override (string, bool)[] RequiredPermissions
        {
            get
            {
                var permissions = new List<(string, bool)>();

                // When targeting Android 11 or lower, AccessFineLocation is required for Bluetooth.
                // For Android 12 and above, it is optional.
                if (SdkVersion.IsBuildVersionLower(BuildVersionCodes.R) ||
                    EssentialsPermissions.IsDeclaredInManifest(Manifest.Permission.AccessFineLocation))
                {
                    permissions.Add((Manifest.Permission.AccessFineLocation, true));
                }

#if __ANDROID_31__
                if (SdkVersion.IsBuildVersionAtLeast(BuildVersionCodes.S) &&
                    SdkVersion.IsDeviceVersionAtLeast(BuildVersionCodes.S))
                {
                    if (EssentialsPermissions.IsDeclaredInManifest(Manifest.Permission.BluetoothScan))
                    {
                        permissions.Add((Manifest.Permission.BluetoothScan, true));
                    }

                    if (EssentialsPermissions.IsDeclaredInManifest(Manifest.Permission.BluetoothConnect))
                    {
                        permissions.Add((Manifest.Permission.BluetoothConnect, true));
                    }

                    if (EssentialsPermissions.IsDeclaredInManifest(Manifest.Permission.BluetoothAdvertise))
                    {
                        permissions.Add((Manifest.Permission.BluetoothAdvertise, true));
                    }
                }
#endif

                return permissions.ToArray();
            }
        }
    }
}