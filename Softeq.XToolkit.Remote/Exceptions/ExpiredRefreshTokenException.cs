// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Remote.Exceptions
{
    /// <summary>
    ///     Represents errors that occur when refreshing token.
    /// </summary>
    public class ExpiredRefreshTokenException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ExpiredRefreshTokenException"/> class.
        /// </summary>
        /// <param name="innerException">Inner exception.</param>
        public ExpiredRefreshTokenException(Exception innerException)
            : base("Refresh Token was expired", innerException)
        {
        }
    }
}
