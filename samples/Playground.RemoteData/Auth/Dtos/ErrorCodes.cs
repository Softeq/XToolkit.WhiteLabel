// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Runtime.Serialization;

namespace Playground.RemoteData.Auth.Dtos
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
