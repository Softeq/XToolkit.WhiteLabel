// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading;
using System.Threading.Tasks;
using Playground.RemoteData.Auth.Dtos;
using Refit;

namespace Playground.RemoteData.Auth
{
    /// <summary>
    ///     API Client for https://github.com/Softeq/NetKit.Auth.
    /// </summary>
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
