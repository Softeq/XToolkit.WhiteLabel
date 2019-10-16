using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Logger;

namespace Softeq.XToolkit.Remote.Handlers
{
    internal class HttpDiagnosticsHandler : DelegatingHandler
    {
        private readonly ILogger _logger;

        public HttpDiagnosticsHandler(HttpMessageHandler innerHandler, ILogger logger = null) : base(innerHandler)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var totalElapsedTime = Stopwatch.StartNew();

            WriteMessage($"Request: {request}");
            if (request.Content != null)
            {
                var content = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
                WriteMessage($"Request Content: {content}");
            }

            var responseElapsedTime = Stopwatch.StartNew();
            var response = await base.SendAsync(request, cancellationToken);

            WriteMessage($"Response: {response}");
            if (response.Content != null)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                WriteMessage($"Response Content: {content}");
            }

            responseElapsedTime.Stop();
            WriteMessage($"Response elapsed time: {responseElapsedTime.ElapsedMilliseconds} ms");

            totalElapsedTime.Stop();
            WriteMessage($"Total elapsed time: {totalElapsedTime.ElapsedMilliseconds} ms");

            return response;
        }

        private void WriteMessage(string message)
        {
            Debug.WriteLine(message);

            _logger?.Debug(message);
        }
    }
}
