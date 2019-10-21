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
    public class DroidAppInfoService : IAppInfoService
    {
        /// <inheritdoc />
        public Platform Platform => Platform.Android;

        /// <inheritdoc />
        public string Name
        {
            get
            {
                var applicationInfo = Context.ApplicationInfo;
                var packageManager = Context.PackageManager;
                return applicationInfo.LoadLabel(packageManager);
            }
        }

        /// <inheritdoc />
        public string PackageName => Context.PackageName;

        /// <inheritdoc />
        public string Version => GetPackageInfo().VersionName;

        /// <inheritdoc />
        public string Build => GetPackageInfo().VersionCode.ToString(CultureInfo.InvariantCulture);

        public string GetVersion(bool withBuildNumber) => withBuildNumber ? $"{Version}.{Build}" : Version;

        private static Context Context => Application.Context;

        private PackageInfo GetPackageInfo() => Context.PackageManager.GetPackageInfo(PackageName, PackageInfoFlags.MetaData);
    }
}
