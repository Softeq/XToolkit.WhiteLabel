// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Model;

namespace Softeq.XToolkit.WhiteLabel.Droid.Services
{
    public class AppService : IAppService
    {
        public Platform Platform => Platform.Android;

        public string GetVersion(bool withBuildNumber)
        {
            var context = Application.Context;
            var info = context.PackageManager.GetPackageInfo(context.PackageName, 0);
            return withBuildNumber
                ? $"{info.VersionName}.{info.VersionCode}"
                : info.VersionName;
        }
    }
}