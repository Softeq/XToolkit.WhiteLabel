using RemoteApp.Services.Profile.Dtos;
using RemoteApp.Services.Profile.Models;

namespace RemoteApp.Services.Profile
{
    public static class Mapper
    {
        public static ProfileResult Map(ProfileResponse x)
        {
            return new ProfileResult
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Location = x.Location,
                PhotoUrl = x.PhotoUrl
            };
        }
    }
}
