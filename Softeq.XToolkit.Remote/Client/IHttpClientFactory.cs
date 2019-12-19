using System.Net.Http;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote.Auth;

namespace Softeq.XToolkit.Remote.Client
{
    public interface IHttpClientFactory
    {
        HttpClient CreateClient(string baseUrl, ILogger logger = null);
        HttpClient CreateAuthClient(string baseUrl, ISessionContext sessionContext, ILogger logger = null);
    }
}
