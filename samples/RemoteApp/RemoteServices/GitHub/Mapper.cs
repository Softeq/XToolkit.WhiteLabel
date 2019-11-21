using RemoteServices.GitHub.Dtos;
using RemoteServices.GitHub.Models;

namespace RemoteServices.GitHub
{
    public static class Mapper
    {
        public static User Map(UserDto dto)
        {
            return new User
            {
                Name = dto.Name
            };
        }
    }
}
