// Developed by Softeq Development Corporation
// http://www.softeq.com

#if __ANDROID_33__
using Android;
using Android.OS;
using Xamarin.Essentials;
#endif
using BasePlatformPermission = Xamarin.Essentials.Permissions.BasePlatformPermission;

namespace Softeq.XToolkit.Permissions.Droid
{
    public class NotificationsPlatformPermission : BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions
        {
            get
            {
                #if __ANDROID_33__
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu
                    && Platform.AppContext.ApplicationInfo?.TargetSdkVersion >= BuildVersionCodes.Tiramisu)
                {
                    return new[] { (Manifest.Permission.PostNotifications, true) };
                }
                #endif

                return new (string, bool)[] { };
            }
        }
    }
}
