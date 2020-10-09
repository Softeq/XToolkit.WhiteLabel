// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading;

namespace Softeq.XToolkit.Remote.Primitives
{
    /// <summary>
    ///     Declares request options used for making requests via <see cref="RemoteService{T}"/>.
    /// </summary>
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
        ///     Gets or sets default <see cref="RequestOptions"/> object.
        ///     If defined, it will be used when no options are specified.
        /// </summary>
        public static RequestOptions? DefaultRequestOptions { get; set; } = default!;

        /// <summary>
        ///     Gets default should retry condition.
        ///     The value is "() => true" - always retry.
        /// </summary>
        public static Func<Exception, bool> DefaultShouldRetry { get; } = _ => true;

        /// <summary>
        ///     Gets default cancellation token.
        ///     The value is <see cref="P:System.Threading.CancellationToken.None"/>.
        /// </summary>
        public static CancellationToken DefaultCancellationToken { get; } = CancellationToken.None;

        /// <summary>
        ///     Gets or sets the number of request retries when errors happen.
        ///     Default value is <see cref="DefaultRetryCount"/>.
        /// </summary>
        public int RetryCount { get; set; } = DefaultRetryCount;

        /// <summary>
        ///     Gets or sets the timeout value in seconds.
        ///     Default value is <see cref="DefaultTimeout"/>.
        /// </summary>
        public int Timeout { get; set; } = DefaultTimeout;

        /// <summary>
        ///     Gets or sets the condition when the request should be retried.
        ///     Default value is <see cref="DefaultShouldRetry"/>.
        /// </summary>
        public Func<Exception, bool> ShouldRetry { get; set; } = DefaultShouldRetry;

        /// <summary>
        ///     Gets or sets token for canceling the request.
        /// </summary>
        public CancellationToken CancellationToken { get; set; } = DefaultCancellationToken;

        /// <summary>
        ///     Returns default request options.
        /// </summary>
        /// <returns>Default request options.</returns>
        public static RequestOptions GetDefaultOptions() => DefaultRequestOptions ?? new RequestOptions();
    }
}
