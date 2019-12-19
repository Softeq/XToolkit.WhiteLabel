namespace RemoteServices.Auth.Models
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
