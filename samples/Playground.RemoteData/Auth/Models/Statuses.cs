// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Playground.RemoteData.Auth.Models
{
    public enum LoginStatus
    {
        Failed,
        Successful,
        EmailOrPasswordIncorrect,
        EmailNotConfirmed,
        Undefined
    }

    public enum RefreshTokenStatus
    {
        Failed,
        Successful
    }
}
