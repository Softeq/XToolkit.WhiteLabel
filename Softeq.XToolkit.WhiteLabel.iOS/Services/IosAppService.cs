// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Model;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    public class IosAppService : IAppService
    {
        public Platform Platform => Platform.iOS;

        public string Name => GetBundleValue("CFBundleDisplayName") ?? GetBundleValue("CFBundleName");

        public string PackageName => GetBundleValue("CFBundleIdentifier");

        public string Version => GetBundleValue("CFBundleShortVersionString");

        public string Build => GetBundleValue("CFBundleVersion");

        public string GetVersion(bool withBuildNumber) => withBuildNumber ? $"{Version}.{Build}" : Version;

        private static string GetBundleValue(string key) => NSBundle.MainBundle.ObjectForInfoDictionary(key)?.ToString();
    }
}