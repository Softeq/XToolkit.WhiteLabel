using System.Runtime.Serialization;

namespace RemoteApp.Services.Auth.Dtos
{
    public enum ErrorDescriptions
    {
        [EnumMember(Value = "invalid_username_or_password")]
        InvalidUsernameOrPassword
    }
}