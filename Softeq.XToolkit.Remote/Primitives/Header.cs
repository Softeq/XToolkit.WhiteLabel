// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Remote.Primitives
{
    /// <summary>
    ///    Common headers for Refit interfaces.
    /// </summary>
    public static class Header
    {
        /// <summary>
        ///     The "Accept-Encoding" header advertises that the client can understand Gzip compression.
        /// </summary>
        public const string Gzip = "Accept-Encoding: gzip";

        /// <summary>
        ///     The "Accept" header advertises that the client can understand JSON format.
        /// </summary>
        public const string Json = "Accept: application/json";

        /// <summary>
        ///     The "Authentication" header with Bearer type.
        /// </summary>
        public const string BearerAuth = "Authorization: Bearer";
    }
}
