// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Playground.RemoteData.GitHub.Dtos
{
    public class UserDetailsResponse
    {
        public string AvatarUrl { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }

    public class UserResponse
    {
        public string AvatarUrl { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
    }
}
