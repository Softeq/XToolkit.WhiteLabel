// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote.Primitives;

namespace Softeq.XToolkit.Remote.Client.Handlers
{
    public class HttpDiagnosticsHandler : DelegatingHandler
    {
        private readonly ILogger _logger;
        private readonly LogVerbosity _verbosity;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HttpDiagnosticsHandler"/> class.
        /// </summary>
        /// <param name="logger">Instance of <see cref="ILogger"/>.</param>
        /// <param name="verbosity">
        ///     Instance of verbosity bitmask, setting the instance verbosity overrides <see cref="DefaultVerbosity"/>.
        /// </param>
        public HttpDiagnosticsHandler(ILogger logger, LogVerbosity verbosity)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _verbosity = verbosity == LogVerbosity.Unspecified ? DefaultVerbosity : verbosity;
        }

        /// <summary>
        ///     Gets or sets default verbosity bitmask <see cref="LogVerbosity"/>.
        /// </summary>
        public static LogVerbosity DefaultVerbosity { get; set; } = LogVerbosity.All;

        /// <inheritdoc />
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var totalElapsedTime = Stopwatch.StartNew();

            await LogHttpRequestAsync(request).ConfigureAwait(false);

            var responseElapsedTime = Stopwatch.StartNew();

            var response = await base.SendAsync(request, cancellationToken);

            await LogHttpResponseAsync(response);

            responseElapsedTime.Stop();
            Log($"Response elapsed time: {responseElapsedTime.ElapsedMilliseconds} ms");

            totalElapsedTime.Stop();
            Log($"Total elapsed time: {totalElapsedTime.ElapsedMilliseconds} ms");

            return response;
        }

        protected virtual async Task LogHttpRequestAsync(HttpRequestMessage request)
        {
            if (_verbosity.HasFlag(LogVerbosity.RequestHeaders))
            {
                Log($"Request: {request}");
            }

            if (_verbosity.HasFlag(LogVerbosity.RequestBody) && request.Content != null)
            {
                var content = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
                Log($"Request Content: {content}");
            }
        }

        protected virtual async Task LogHttpResponseAsync(HttpResponseMessage? response)
        {
            if (_verbosity.HasFlag(LogVerbosity.ResponseHeaders))
            {
                Log($"Response: {response}");
            }

            if (_verbosity.HasFlag(LogVerbosity.ResponseBody) && response != null)
            {
                if (response.Content?.Headers.ContentLength == null)
                {
                    Log("Response Content or Content-Length: null");
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    Log($"Response Content: {content}");
                }
            }
        }

        private void Log(string message)
        {
            _logger.Debug(message);
        }
    }
}
