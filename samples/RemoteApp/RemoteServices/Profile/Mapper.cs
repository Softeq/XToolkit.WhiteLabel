// Developed by Softeq Development Corporation
// http://www.softeq.com

using RemoteServices.Profile.Dtos;
using RemoteServices.Profile.Models;

namespace RemoteServices.Profile
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
