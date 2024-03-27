// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.OS;
using Microsoft.Maui.ApplicationModel;

namespace Softeq.XToolkit.Permissions.Droid
{
    internal static class SdkVersion
    {
        public static bool IsTargetSdkLower(BuildVersionCodes versionCode)
        {
            return Platform.AppContext.ApplicationInfo?.TargetSdkVersion <= versionCode;
        }

        public static bool IsTargetSdkAtLeast(BuildVersionCodes versionCode)
        {
            return Platform.AppContext.ApplicationInfo?.TargetSdkVersion >= versionCode;
        }

        public static bool IsBuildSdkAtLeast(BuildVersionCodes versionCode)
        {
            return Build.VERSION.SdkInt >= versionCode;
        }
    }
}