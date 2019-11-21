using System.Net.Http;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote.Auth;

namespace Softeq.XToolkit.Remote.Client
{
    public interface IHttpClientFactory
    {
        HttpClient CreateSimpleClient(string baseUrl, ILogger logger = null);
        HttpClient CreateAuthClient(string baseUrl, ISessionContext sessionContext, ILogger logger = null);
    }
}
