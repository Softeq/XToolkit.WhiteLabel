using System.Threading;
using System.Threading.Tasks;
using Refit;
using RemoteApp.Services.Auth.Dtos;

namespace RemoteApp.Services.Auth
{
    public interface IAuthApiService
    {
        [Post("/connect/token")]
        Task<TokenResponse> Login(
            [Body(BodySerializationMethod.UrlEncoded)] LoginRequest requestDto,
            CancellationToken cancellationToken);

        [Post("/connect/token")]
        Task<TokenResponse> RefreshToken(
            [Body(BodySerializationMethod.UrlEncoded)] RefreshTokenRequest requestDto,
            CancellationToken cancellationToken);
    }
}
