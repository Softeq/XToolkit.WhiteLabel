// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Remote.Exceptions
{
    /// <summary>
    ///     Represents errors that occur during refresh token.
    /// </summary>
    public class ExpiredRefreshTokenException : Exception
    {
        public ExpiredRefreshTokenException(Exception innerException)
            : base("Refresh Token was expired", innerException)
        {
        }
    }
}
