using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Logger;

namespace Softeq.XToolkit.Remote.Handlers
{
    public class HttpDiagnosticsHandler : DelegatingHandler
    {
        private readonly ILogger _logger;

        public HttpDiagnosticsHandler(HttpMessageHandler innerHandler, ILogger logger = null) : base(innerHandler)
        {
            _logger = logger;
        }

        public HttpDiagnosticsHandler()
        { }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            WriteMessage($"Request: {request}");
            if (request.Content != null)
            {
                var content = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
                WriteMessage($"Request Content: {content}");
            }

            var responseElapsedTime = Stopwatch.StartNew();
            var response = await base.SendAsync(request, cancellationToken);

            responseElapsedTime.Stop();
            WriteMessage($"Response elapsed time: {responseElapsedTime.ElapsedMilliseconds} ms");
            WriteMessage(response.ToString());

            return response;
        }

        private void WriteMessage(string message)
        {
            Debug.WriteLine(message);

            _logger?.Debug(message);
        }
    }
}
