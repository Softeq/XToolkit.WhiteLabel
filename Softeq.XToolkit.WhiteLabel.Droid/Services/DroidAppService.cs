// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Globalization;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Model;

namespace Softeq.XToolkit.WhiteLabel.Droid.Services
{
    public class DroidAppService : IAppService
    {
        public Platform Platform => Platform.Android;

        public string Name
        {
            get
            {
                var applicationInfo = Context.ApplicationInfo;
                var packageManager = Context.PackageManager;
                return applicationInfo.LoadLabel(packageManager);
            }
        }

        public string PackageName => Context.PackageName;

        public string Version => GetPackageInfo().VersionName;

        public string Build => GetPackageInfo().VersionCode.ToString(CultureInfo.InvariantCulture);

        public string GetVersion(bool withBuildNumber) => withBuildNumber ? $"{Version}.{Build}" : Version;

        private Context Context => Application.Context;

        private PackageInfo GetPackageInfo()
        {
            var packageManager = Context.PackageManager;
            return packageManager.GetPackageInfo(PackageName, PackageInfoFlags.MetaData);
        }
    }
}