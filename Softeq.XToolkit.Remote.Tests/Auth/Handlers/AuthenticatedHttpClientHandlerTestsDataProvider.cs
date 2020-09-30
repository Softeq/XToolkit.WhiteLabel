// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Remote.Tests.Auth.Handlers
{
    public static class AuthenticatedHttpClientHandlerTestsDataProvider
    {
        private const string TestAuthHeaderSchema = "test-auth";
        public const string TestAuthTokenValue = "test-token-xxx";

        public static Task<string> CreateAccessTokenResult()
        {
            return Task.FromResult(TestAuthTokenValue);
        }

        public static AuthenticationHeaderValue CreateAuthHeader()
        {
            return new AuthenticationHeaderValue(TestAuthHeaderSchema);
        }
    }
}
