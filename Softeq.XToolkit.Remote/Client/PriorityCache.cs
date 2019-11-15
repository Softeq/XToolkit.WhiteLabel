using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fusillade;
using Punchclock;

namespace Softeq.XToolkit.Remote.Client
{
    /// <summary>
    /// Handles caching for our Http requests.
    /// </summary>
    public static class PriorityCache
    {
        /// <summary>
        /// Initializes static members of the <see cref="PriorityCache"/> class.
        /// </summary>
        static PriorityCache()
        {
            var innerHandler = HttpHandlerBuilder.NativeHandler;

            Speculative = new CustomRateLimitedHttpMessageHandler(innerHandler, Priority.Speculative, 0, 1048576 * 5);
            UserInitiated = new CustomRateLimitedHttpMessageHandler(innerHandler, Priority.UserInitiated, 0);
            Background = new CustomRateLimitedHttpMessageHandler(innerHandler, Priority.Background, 0);
        }

        /// <summary>
        /// Gets or sets a handler of that allow a certain number of bytes to be
        /// read before cancelling all future requests. This is designed for
        /// reading data that may or may not be used by the user later, in order
        /// to improve response times should the user later request the data.
        /// </summary>
        public static DelegatingHandler Speculative { get; set; }

        /// <summary>
        /// Gets or sets a scheduler that should be used for requests initiated by a user
        /// action such as clicking an item, they have the highest priority.
        /// </summary>
        public static DelegatingHandler UserInitiated { get; set; }

        /// <summary>
        /// Gets or sets a scheduler that should be used for requests initiated in the
        /// background, and are scheduled at a lower priority.
        /// </summary>
        public static DelegatingHandler Background { get; set; }
    }

    public class CustomRateLimitedHttpMessageHandler : LimitingHttpMessageHandler
    {
        private readonly int _priority;
        private readonly OperationQueue _opQueue;
        private readonly Dictionary<string, InflightRequest> _inflightResponses =
            new Dictionary<string, InflightRequest>();

        private readonly Func<HttpRequestMessage, HttpResponseMessage, string, CancellationToken, Task> _cacheResult;

        private long? _maxBytesToRead;

        /// <summary>
        /// Initializes a new instance of the <see cref="RateLimitedHttpMessageHandler"/> class.
        /// </summary>
        /// <param name="handler">The handler we are wrapping.</param>
        /// <param name="basePriority">The base priority of the request.</param>
        /// <param name="priority">The priority of the request.</param>
        /// <param name="maxBytesToRead">The maximum number of bytes we can reead.</param>
        /// <param name="opQueue">The operation queue on which to run the operation.</param>
        /// <param name="cacheResultFunc">A method that is called if we need to get cached results.</param>
        public CustomRateLimitedHttpMessageHandler(HttpMessageHandler handler, Priority basePriority, int priority = 0, long? maxBytesToRead = null, OperationQueue opQueue = null, Func<HttpRequestMessage, HttpResponseMessage, string, CancellationToken, Task> cacheResultFunc = null)
            : base(handler)
        {
            _priority = (int) basePriority + priority;
            _maxBytesToRead = maxBytesToRead;
            _opQueue = opQueue;
            _cacheResult = cacheResultFunc;
        }

        /// <summary>
        /// Generates a unique key for for a <see cref="HttpRequestMessage"/>.
        /// This assists with the caching.
        /// </summary>
        /// <param name="request">The request to generate a unique key for.</param>
        /// <returns>The unique key.</returns>
        public static string UniqueKeyForRequest(HttpRequestMessage request)
        {
            var ret = new[]
            {
                request.RequestUri.ToString(),
                request.Method.Method,
                request.Headers.Accept.ConcatenateAll(x => x.CharSet + x.MediaType),
                request.Headers.AcceptEncoding.ConcatenateAll(x => x.Value),
                (request.Headers.Referrer ?? new Uri("http://example")).AbsoluteUri,
                request.Headers.UserAgent.ConcatenateAll(x => (x.Product != null ? x.Product.ToString() : x.Comment)),
            }.Aggregate(
                new StringBuilder(),
                (acc, x) =>
                {
                    acc.AppendLine(x);
                    return acc;
                });

            if (request.Headers.Authorization != null)
            {
                ret.AppendLine(request.Headers.Authorization.Parameter + request.Headers.Authorization.Scheme);
            }

            return "HttpSchedulerCache_" + ret.ToString().GetHashCode().ToString("x", CultureInfo.InvariantCulture);
        }

        /// <inheritdoc />
        public override void ResetLimit(long? maxBytesToRead = null)
        {
            _maxBytesToRead = maxBytesToRead;
        }

        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var method = request.Method;
            if (method != HttpMethod.Get && method != HttpMethod.Head && method != HttpMethod.Options)
            {
                return base.SendAsync(request, cancellationToken);
            }

            var cacheResult = _cacheResult;
            if (cacheResult == null && NetCache.RequestCache != null)
            {
                cacheResult = NetCache.RequestCache.Save;
            }

            if (_maxBytesToRead != null && _maxBytesToRead.Value < 0)
            {
                var tcs = new TaskCompletionSource<HttpResponseMessage>();
                tcs.SetCanceled();
                return tcs.Task;
            }

            var key = UniqueKeyForRequest(request);
            var realToken = new CancellationTokenSource();
            var ret = new InflightRequest(() =>
            {
                lock (_inflightResponses)
                {
                    _inflightResponses.Remove(key);
                }

                realToken.Cancel();
            });

            lock (_inflightResponses)
            {
                if (_inflightResponses.ContainsKey(key))
                {
                    var val = _inflightResponses[key];
                    val.AddRef();
                    cancellationToken.Register(val.Cancel);

                    return val.Response.ToTask(cancellationToken);
                }

                _inflightResponses[key] = ret;
            }

            cancellationToken.Register(ret.Cancel);

            var queue = _opQueue ?? NetCache.OperationQueue;

            queue.Enqueue(
                _priority,
                null,
                async () =>
                {
                    try
                    {
                        var resp = await base.SendAsync(request, realToken.Token).ConfigureAwait(false);

                        if (_maxBytesToRead != null && resp.Content != null && resp.Content.Headers.ContentLength != null)
                        {
                            _maxBytesToRead -= resp.Content.Headers.ContentLength;
                        }

                        if (cacheResult != null && resp.Content != null)
                        {
                            var ms = new MemoryStream();
                            var stream = await resp.Content.ReadAsStreamAsync().ConfigureAwait(false);
                            await stream.CopyToAsync(ms, 32 * 1024, realToken.Token).ConfigureAwait(false);

                            realToken.Token.ThrowIfCancellationRequested();

                            var newResp = new HttpResponseMessage();
                            foreach (var kvp in resp.Headers)
                            {
                                newResp.Headers.Add(kvp.Key, kvp.Value);
                            }

                            var newContent = new ByteArrayContent(ms.ToArray());
                            foreach (var kvp in resp.Content.Headers)
                            {
                                newContent.Headers.Add(kvp.Key, kvp.Value);
                            }

                            newResp.Content = newContent;

                            resp = newResp;
                            await cacheResult(request, resp, key, realToken.Token).ConfigureAwait(false);
                        }

                        return resp;
                    }
                    finally
                    {
                        lock (_inflightResponses)
                        {
                            _inflightResponses.Remove(key);
                        }
                    }
                },
                realToken.Token).ToObservable().Subscribe(ret.Response);

            return ret.Response.ToTask(cancellationToken);
        }
    }

    internal class InflightRequest
    {
        private int _refCount = 1;
        private Action _onCancelled;

        public InflightRequest(Action onFullyCancelled)
        {
            _onCancelled = onFullyCancelled;
            Response = new AsyncSubject<HttpResponseMessage>();
        }

        public AsyncSubject<HttpResponseMessage> Response { get; protected set; }

        public void AddRef()
        {
            Interlocked.Increment(ref _refCount);
        }

        public void Cancel()
        {
            if (Interlocked.Decrement(ref _refCount) <= 0)
            {
                _onCancelled();
            }
        }
    }

    internal static class ConcatenateMixin
    {
        public static string ConcatenateAll<T>(this IEnumerable<T> enumerables, Func<T, string> selector, char separator = '|')
        {
            return enumerables.Aggregate(new StringBuilder(), (acc, x) =>
            {
                acc.Append(selector(x)).Append(separator);
                return acc;
            }).ToString();
        }
    }
}
