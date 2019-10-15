using System;
using System.Threading;

namespace Softeq.XToolkit.Remote.Primitives
{
    public class RequestOptions
    {
        /// <summary>
        ///     Default RequestOptions object. If defined, it will be used when no options are specified
        /// </summary>
        public static RequestOptions DefaultRequestOptions { get; set; }

        /// <summary>
        ///     Default value for retry intents
        /// </summary>
        public static int DefaultRetryCount { get; set; } = 1;

        /// <summary>
        ///     Default timeout for requests. In seconds.
        /// </summary>
        public static int DefaultTimeout { get; set; } = 1;

        /// <summary>
        ///     Default priority for requests. Matches fusillade policy
        /// </summary>
        public static Priority DefaultPriority { get; set; } = Priority.UserInitiated;

        /// <summary>
        ///     Default should retry condition. Default value is null
        /// </summary>
        public static Func<Exception, bool> DefaultShouldRetry { get; set; }

        /// <summary>
        ///     Default cancellation token. Default value is None.
        /// </summary>
        public static CancellationToken DefaultCancellationToken { get; set; } = CancellationToken.None;

        public int RetryCount { get; set; } = DefaultRetryCount;

        public int Timeout { get; set; } = DefaultTimeout;

        public Priority Priority { get; set; } = DefaultPriority;

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
