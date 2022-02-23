// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Net;
using Softeq.XToolkit.WhiteLabel.Interfaces;

namespace Softeq.XToolkit.WhiteLabel.Services
{
    /// <summary>
    ///     Provides methods for encoding and decoding URLs based on <see cref="T:System.Net.WebUtility"/>.
    /// </summary>
    public class UrlDecoder : IUrlDecoder
    {
        /// <inheritdoc />
        public string DecodeUrl(string input)
        {
            return WebUtility.UrlDecode(input);
        }

        /// <inheritdoc />
        public string EncodeUrl(string input)
        {
            return WebUtility.UrlEncode(input) ?? string.Empty;
        }
    }
}
