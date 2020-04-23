// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace RemoteServices.Profile.Dtos
{
    public class ProfileResponse
    {
        public string ProfileId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public int Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
