using System.Runtime.Serialization;

namespace RemoteServices.Auth.Dtos
{
    public enum ErrorCodes
    {
        [EnumMember(Value = "invalid_client")]
        InvalidClient,

        [EnumMember(Value = "unsupported_grant_type")]
        UnsupportedGradType,

        [EnumMember(Value = "invalid_grant")]
        InvalidGrant
    }
}