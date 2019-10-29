using System.Threading;
using System.Threading.Tasks;
using RemoteApp.Services.Auth.Dtos;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Client;

namespace RemoteApp.Services.Auth
{
    public class LoginService
    {
        private readonly IRemoteService<ISofteqAuthApiService> _remoteService;
        private readonly AuthConfig _config;

        public LoginService(IRemoteServiceFactory remoteServiceFactory)
        {
            var httpClientBuilder = new HttpClientBuilder("https://softeq-netkit-dev-auth-webapp.azurewebsites.net");

            _remoteService = remoteServiceFactory.Create<ISofteqAuthApiService>(httpClientBuilder);

            _config = new AuthConfig
            {
                ClientId = "ro.client",
                ClientSecret = "secret"
            };
        }

        public async Task Login(string login, string password, CancellationToken cancellationToken)
        {
            var request = new LoginRequest
            {
                UserName = login,
                Password = password,
                ClientId = _config.ClientId,
                ClientSecret = _config.ClientSecret
            };

            var response = await _remoteService
                .Execute((service, ct) => service.Login(request, ct))
                .ConfigureAwait(false);

            // TODO YP: handle errors 400..

            // TODO YP: add mapper
        }

        public async Task RefreshToken(string refreshToken, CancellationToken cancellationToken)
        {
            var request = new RefreshTokenRequest
            {
                RefreshToken = refreshToken,
                ClientId = _config.ClientId,
                ClientSecret = _config.ClientSecret
            };

            var response = await _remoteService
                .Execute((service, ct) => service.RefreshToken(request, ct))
                .ConfigureAwait(false);

        }

        public class AuthConfig
        {
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
        }
    }
}
