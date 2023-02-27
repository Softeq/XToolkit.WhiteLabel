// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.OS;
using Microsoft.Maui.ApplicationModel;

namespace Softeq.XToolkit.Permissions.Droid
{
    public static class SdkVersion
    {
        public static bool IsBuildVersionLower(BuildVersionCodes versionCode)
        {
            return Platform.AppContext.ApplicationInfo?.TargetSdkVersion <= versionCode;
        }

        public static bool IsBuildVersionAtLeast(BuildVersionCodes versionCode)
        {
            return Platform.AppContext.ApplicationInfo?.TargetSdkVersion >= versionCode;
        }

        public static bool IsDeviceVersionAtLeast(BuildVersionCodes versionCode)
        {
            return Build.VERSION.SdkInt >= versionCode;
        }
    }
}