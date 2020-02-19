// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Model;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    public class IosAppInfoService : IAppInfoService
    {
        /// <inheritdoc />
        public Platform Platform => Platform.iOS;

        /// <inheritdoc />
        public string Name => GetBundleValue("CFBundleDisplayName") ?? GetBundleValue("CFBundleName")!;

        /// <inheritdoc />
        public string PackageName => GetBundleValue("CFBundleIdentifier")!;

        /// <inheritdoc />
        public string Version => GetBundleValue("CFBundleShortVersionString")!;

        /// <inheritdoc />
        public string Build => GetBundleValue("CFBundleVersion")!;

        public string GetVersion(bool withBuildNumber) => withBuildNumber ? $"{Version}.{Build}" : Version;

        private static string? GetBundleValue(string key) => NSBundle.MainBundle.ObjectForInfoDictionary(key)?.ToString();
    }
}
