using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Remote.Client.Handlers
{
    internal class RefreshTokenHttpClientHandler : DelegatingHandler
    {
        private readonly Func<Task<string>> _getAccessToken;
        private readonly Func<Task<string>> _getRefreshedToken;

        public RefreshTokenHttpClientHandler(
            Func<Task<string>> getAccessToken,
            Func<Task<string>> getRefreshedToken)
        {
            _getAccessToken = getAccessToken ?? throw new ArgumentNullException(nameof(getAccessToken));

            _getRefreshedToken = getRefreshedToken;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage result;

            if (request.Headers.Authorization != null)
            {
                var token = await _getAccessToken().ConfigureAwait(false);

                result = await SendWithTokenAsync(request, cancellationToken, token).ConfigureAwait(false);

                if (result.StatusCode == HttpStatusCode.Unauthorized && _getRefreshedToken != null)
                {
                    var refreshedToken = await _getRefreshedToken().ConfigureAwait(false);

                    result = await SendWithTokenAsync(request, cancellationToken, refreshedToken).ConfigureAwait(false);
                }
            }
            else
            {
                result = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            return result;
        }

        private async Task<HttpResponseMessage> SendWithTokenAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken,
            string token)
        {
            var auth = request.Headers.Authorization;
            if (auth != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, token);
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
