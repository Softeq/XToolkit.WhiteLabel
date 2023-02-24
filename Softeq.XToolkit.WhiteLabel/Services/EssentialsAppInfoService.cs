// Developed by Softeq Development Corporation
// http://www.softeq.com

using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Platform = Softeq.XToolkit.WhiteLabel.Model.Platform;

namespace Softeq.XToolkit.WhiteLabel.Services
{
    /// <summary>
    ///     Maui.Essentials thread-safe implementation for <see cref="IAppInfoService"/> interface.
    /// </summary>
    public class EssentialsAppInfoService : IAppInfoService
    {
        /// <inheritdoc />
        public virtual Platform Platform
        {
            get
            {
                var devicePlatform = DeviceInfo.Platform;

                if (devicePlatform == DevicePlatform.iOS)
                {
                    return Platform.iOS;
                }

                if (devicePlatform == DevicePlatform.Android)
                {
                    return Platform.Android;
                }

                if (devicePlatform == DevicePlatform.macOS)
                {
                    return Platform.macOS;
                }

                if (devicePlatform == DevicePlatform.Tizen)
                {
                    return Platform.Tizen;
                }

                if (devicePlatform == DevicePlatform.tvOS)
                {
                    return Platform.tvOS;
                }

                if (devicePlatform == DevicePlatform.watchOS)
                {
                    return Platform.watchOS;
                }

                if (devicePlatform == DevicePlatform.WinUI)
                {
                    return Platform.WinUI;
                }

                if (devicePlatform == DevicePlatform.MacCatalyst)
                {
                    return Platform.MacCatalyst;
                }

                return Platform.Unknown;
            }
        }

        /// <inheritdoc />
        public string Name => AppInfo.Name;

        /// <inheritdoc />
        public string PackageName => AppInfo.PackageName;

        /// <inheritdoc />
        public string Version => AppInfo.VersionString;

        /// <inheritdoc />
        public string Build => AppInfo.BuildString;
    }
}
