using Newtonsoft.Json;

namespace RemoteApp.Services.Auth.Dtos
{
    public class ErrorResponse
    {
        [JsonProperty("error")]
        public ErrorCodes ErrorCode { get; set; }

        [JsonProperty("error_description")]
        public ErrorDescriptions ErrorDescription { get; set; }
    }
}
