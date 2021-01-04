// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Runtime.Serialization;

namespace Playground.RemoteData.Auth.Dtos
{
    public enum ErrorDescriptions
    {
        [EnumMember(Value = "invalid_username_or_password")]
        InvalidUsernameOrPassword
    }
}
