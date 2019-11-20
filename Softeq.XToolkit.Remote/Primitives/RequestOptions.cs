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
        ///     Default priority for requests. Matches fusillade policy.
        /// </summary>
        public const RequestPriority DefaultPriority = RequestPriority.Speculative;

        /// <summary>
        ///     Default RequestOptions object. If defined, it will be used when no options are specified.
        /// </summary>
        public static RequestOptions DefaultRequestOptions { get; set; }

        /// <summary>
        ///     Default should retry condition. Default value is null.
        /// </summary>
        public static Func<Exception, bool> DefaultShouldRetry { get; }

        /// <summary>
        ///     Default cancellation token. Default value is None.
        /// </summary>
        public static CancellationToken DefaultCancellationToken { get; } = CancellationToken.None;

        public int RetryCount { get; set; } = DefaultRetryCount;

        public int Timeout { get; set; } = DefaultTimeout;

        public RequestPriority Priority { get; set; } = DefaultPriority;

        public Func<Exception, bool> ShouldRetry { get; set; } = DefaultShouldRetry;

        public CancellationToken CancellationToken { get; set; } = DefaultCancellationToken;

        public static RequestOptions GetDefaultOptions()
        {
            return DefaultRequestOptions
                   ?? new RequestOptions
                   {
                       Priority = DefaultPriority,
                       RetryCount = DefaultRetryCount,
                       Timeout = DefaultTimeout,
                       ShouldRetry = DefaultShouldRetry,
                       CancellationToken = DefaultCancellationToken
                   };
        }
    }
}
