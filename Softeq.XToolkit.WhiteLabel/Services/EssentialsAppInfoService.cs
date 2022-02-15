// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Model;
using Xamarin.Essentials;

namespace Softeq.XToolkit.WhiteLabel.Services
{
    /// <summary>
    ///     Xamarin.Essentials thread-safe implementation for <see cref="IAppInfoService"/> interface.
    /// </summary>
    public abstract class EssentialsAppInfoService : IAppInfoService
    {
        /// <inheritdoc />
        public abstract Platform Platform { get; }

        /// <inheritdoc />
        public string? Name => AppInfo.Name;

        /// <inheritdoc />
        public string? PackageName => AppInfo.PackageName;

        /// <inheritdoc />
        public string? Version => AppInfo.VersionString;

        /// <inheritdoc />
        public string? Build => AppInfo.BuildString;
    }
}
