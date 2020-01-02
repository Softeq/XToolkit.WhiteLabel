namespace RemoteServices.GitHub.Dtos
{
    public class UserDetailsResponse
    {
        public string AvatarUrl { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
    }

    public class UserResponse
    {
        public string AvatarUrl { get; set; }
        public string Login { get; set; }
    }
}
