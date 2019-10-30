using System.Threading.Tasks;
using Refit;
using RemoteApp.Services.Auth.Dtos;
using RemoteApp.Services.Auth.Models;
using ErrorCodes = RemoteApp.Services.Auth.Models.ErrorCodes;

namespace RemoteApp.Services.Auth
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
