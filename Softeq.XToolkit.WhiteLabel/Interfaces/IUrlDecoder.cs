// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Interfaces
{
    /// <summary>
    ///     Provides methods for encoding and decoding URLs.
    /// </summary>
    public interface IUrlDecoder
    {
        /// <summary>
        ///     Converts a string that has been encoded for transmission in a URL into a decoded string.
        /// </summary>
        /// <param name="input">A URL-encoded string to decode.</param>
        /// <returns>A decoded string.</returns>
        string EncodeUrl(string input);

        /// <summary>
        ///     Converts a text string into a URL-encoded string.
        /// </summary>
        /// <param name="input">The text to URL-encode.</param>
        /// <returns>A URL-encoded string.</returns>
        string DecodeUrl(string input);
    }
}
