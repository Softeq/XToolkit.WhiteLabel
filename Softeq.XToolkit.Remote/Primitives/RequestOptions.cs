// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading;

namespace Softeq.XToolkit.Remote.Primitives
{
    public class RequestOptions
    {
        /// <summary>
        ///     Default value for retry intents.
        /// </summary>
        public const int DefaultRetryCount = 1;

        /// <summary>
        ///     Default timeout for requests. In seconds.
        /// </summary>
        public const int DefaultTimeout = 20;

        /// <summary>
        ///     Gets or sets default RequestOptions object. If defined, it will be used when no options are specified.
        /// </summary>
        public static RequestOptions? DefaultRequestOptions { get; set; }

        /// <summary>
        ///     Gets default should retry condition. Default value is null.
        /// </summary>
        public static Func<Exception, bool> DefaultShouldRetry { get; } = _ => true;

        /// <summary>
        ///     Gets default cancellation token. Default value is None.
        /// </summary>
        public static CancellationToken DefaultCancellationToken { get; } = CancellationToken.None;

        public int RetryCount { get; set; } = DefaultRetryCount;

        public int Timeout { get; set; } = DefaultTimeout;

        public Func<Exception, bool> ShouldRetry { get; set; } = DefaultShouldRetry;

        public CancellationToken CancellationToken { get; set; } = DefaultCancellationToken;

        public static RequestOptions GetDefaultOptions()
        {
            return DefaultRequestOptions
                   ?? new RequestOptions
                   {
                       RetryCount = DefaultRetryCount,
                       Timeout = DefaultTimeout,
                       ShouldRetry = DefaultShouldRetry,
                       CancellationToken = DefaultCancellationToken
                   };
        }
    }
}
