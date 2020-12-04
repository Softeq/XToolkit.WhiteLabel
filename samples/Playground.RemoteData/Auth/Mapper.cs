// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Playground.RemoteData.Auth.Dtos;
using Playground.RemoteData.Auth.Models;
using Refit;
using ErrorCodes = Playground.RemoteData.Auth.Models.ErrorCodes;

namespace Playground.RemoteData.Auth
{
    internal static class Mapper
    {
        internal static async Task<AuthException> MapAsync(ApiException exception)
        {
            var errorResponse = await exception.GetContentAsAsync<ErrorResponse>().ConfigureAwait(false);
            var error = new ErrorResult
            {
                ErrorCode = Map(errorResponse.ErrorCode)
            };

            return new AuthException(error, exception);
        }

        internal static TokenResult Map(TokenResponse response)
        {
            return new TokenResult
            {
                AccessToken = response.AccessToken,
                RefreshToken = response.RefreshToken
            };
        }

        internal static LoginStatus Map(ErrorResult error)
        {
            return error.ErrorCode switch
            {
                ErrorCodes.UserNotFound => LoginStatus.EmailOrPasswordIncorrect,
                ErrorCodes.EmailIsNotConfirmed => LoginStatus.EmailNotConfirmed,
                _ => LoginStatus.Failed
            };
        }

        private static ErrorCodes Map(Dtos.ErrorCodes errorCode)
        {
            return errorCode switch
            {
                Dtos.ErrorCodes.InvalidGrant => ErrorCodes.UserNotFound,
                _ => ErrorCodes.UnknownError
            };
        }
    }
}
