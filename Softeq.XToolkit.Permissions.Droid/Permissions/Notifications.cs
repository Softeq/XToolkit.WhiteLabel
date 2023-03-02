// Developed by Softeq Development Corporation
// http://www.softeq.com

#if __ANDROID_33__
using Android;
using Android.OS;
#endif
using EssentialsPermissions = Microsoft.Maui.ApplicationModel.Permissions;

namespace Softeq.XToolkit.Permissions.Droid.Permissions
{
    public class Notifications : EssentialsPermissions.BasePlatformPermission
    {
        public override (string, bool)[] RequiredPermissions
        {
            get
            {
#if __ANDROID_33__
                var isSupport =
                    SdkVersion.IsBuildVersionAtLeast(BuildVersionCodes.Tiramisu) &&
                    SdkVersion.IsDeviceVersionAtLeast(BuildVersionCodes.Tiramisu) &&
                    EssentialsPermissions.IsDeclaredInManifest(Manifest.Permission.PostNotifications);

                return isSupport ?
                    new (string, bool)[] { (Manifest.Permission.PostNotifications, true) } :
                    System.Array.Empty<(string, bool)>();
#else
                return new (string, bool)[] { };
#endif
            }
        }
    }
}