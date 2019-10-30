namespace RemoteApp.Services.Auth.Models
{
    public enum LoginStatus
    {
        Failed,
        Successful,
        EmailOrPasswordIncorrect,
        EmailNotConfirmed,
        Undefined,
    }
}
