using System.Collections.Generic;

namespace RemoteApp.Services.Auth.Dtos
{
    public class ResponseBase
    {
        // 400
        public IReadOnlyCollection<ValidationErrorResponse> ValidationErrors { get; set; }

        // 401-500
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }

    public class ValidationErrorResponse
    {
        public string Field { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
