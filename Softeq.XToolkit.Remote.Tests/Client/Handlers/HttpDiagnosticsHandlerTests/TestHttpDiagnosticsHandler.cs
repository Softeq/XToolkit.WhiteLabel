// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote.Client.Handlers;
using Softeq.XToolkit.Remote.Primitives;

namespace Softeq.XToolkit.Remote.Tests.Client.Handlers.HttpDiagnosticsHandlerTests
{
    internal class TestHttpDiagnosticsHandler : HttpDiagnosticsHandler
    {
        public TestHttpDiagnosticsHandler(ILogger logger, LogVerbosity verbosity)
            : base(logger, verbosity)
        {
        }

        public Task<HttpResponseMessage> PublicSendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return SendAsync(request, cancellationToken);
        }
    }
}
