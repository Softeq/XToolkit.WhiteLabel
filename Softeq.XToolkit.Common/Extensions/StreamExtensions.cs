// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.IO;

namespace Softeq.XToolkit.Common.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="T:System.IO.Stream"/>.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        ///     Converts the <see cref="T:System.IO.Stream"/> to the bytes array.
        ///     For <see cref="T:System.IO.MemoryStream"/> will be used default method ToArray().
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>Bytes array of <see cref="T:System.IO.Stream"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="stream"/> parameter cannot be <see langword="null"/>.
        /// </exception>
        public static byte[] ToArray(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (stream is MemoryStream ms)
            {
                return ms.ToArray();
            }

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
