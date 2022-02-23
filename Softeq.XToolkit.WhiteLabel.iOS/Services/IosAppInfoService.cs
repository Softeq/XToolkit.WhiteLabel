// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Services;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    /// <summary>
    ///     iOS platform-specific extended implementation of <see cref="EssentialsAppInfoService"/> class.
    /// </summary>
    public class IosAppInfoService : EssentialsAppInfoService
    {
        /// <inheritdoc />
        public override Platform Platform => Platform.iOS;
    }
}
