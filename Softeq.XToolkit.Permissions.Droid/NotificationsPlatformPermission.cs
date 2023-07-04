// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android;
using Android.OS;
using BasePlatformPermission = Xamarin.Essentials.Permissions.BasePlatformPermission;

namespace Softeq.XToolkit.Permissions.Droid
{
    public class NotificationsPlatformPermission : BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions
        {
            get
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
                {
                    return new[] { (Manifest.Permission.PostNotifications, true) };
                }

                return new (string, bool)[] { };
            }
        }
    }
}
