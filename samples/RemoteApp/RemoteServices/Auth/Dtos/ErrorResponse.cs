// Developed by Softeq Development Corporation
// http://www.softeq.com

using Newtonsoft.Json;

namespace RemoteServices.Auth.Dtos
{
    public class ErrorResponse
    {
        [JsonProperty("error")]
        public ErrorCodes ErrorCode { get; set; }

        [JsonProperty("error_description")]
        public ErrorDescriptions ErrorDescription { get; set; }
    }
}
