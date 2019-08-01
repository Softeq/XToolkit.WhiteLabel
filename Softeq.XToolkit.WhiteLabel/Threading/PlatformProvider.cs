// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Threading
{
    /// <summary>
    ///     Access the current <see cref="IPlatformProvider" />.
    /// </summary>
    public static class PlatformProvider
    {
        /// <summary>
        ///     Gets or sets the current <see cref="IPlatformProvider" />.
        /// </summary>
        public static IPlatformProvider Current { get; set; }
    }
}
