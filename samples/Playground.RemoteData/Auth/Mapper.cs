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
            if (error == null)
            {
                return LoginStatus.Undefined;
            }

            switch (error.ErrorCode)
            {
                case ErrorCodes.UserNotFound:
                    return LoginStatus.EmailOrPasswordIncorrect;
                case ErrorCodes.EmailIsNotConfirmed:
                    return LoginStatus.EmailNotConfirmed;
                default:
                    return LoginStatus.Failed;
            }
        }

        private static ErrorCodes Map(Dtos.ErrorCodes errorCode)
        {
            switch (errorCode)
            {
                case Dtos.ErrorCodes.InvalidGrant:
                    return ErrorCodes.UserNotFound;
                default:
                    return ErrorCodes.UnknownError;
            }
        }
    }
}
