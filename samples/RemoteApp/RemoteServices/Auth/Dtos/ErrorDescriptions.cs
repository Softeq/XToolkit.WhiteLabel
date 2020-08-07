// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Runtime.Serialization;

namespace RemoteServices.Auth.Dtos
{
    public enum ErrorDescriptions
    {
        [EnumMember(Value = "invalid_username_or_password")]
        InvalidUsernameOrPassword
    }
}