// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.RemoteData.Profile.Dtos;
using Playground.RemoteData.Profile.Models;

namespace Playground.RemoteData.Profile
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
