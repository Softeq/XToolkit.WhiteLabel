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

        public string GetVersion(bool withBuildNumber)
        {
            var version = NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"];
            return withBuildNumber
                ? $"{version}.{NSBundle.MainBundle.InfoDictionary["CFBundleVersion"]}"
                : version.ToString();
        }
    }
}