using System;
using System.Collections.Generic;
using System.Linq;
using RemoteServices.GitHub.Dtos;
using RemoteServices.GitHub.Models;

namespace RemoteServices.GitHub
{
    public static class Mapper
    {
        public static User Map(UserDetailsResponse x)
        {
            return x == null
                ? null
                : new User
                {
                    Name = x.Name
                };
        }

        public static User Map(UserResponse x)
        {
            return x == null
                ? null
                : new User
                {
                    Name = x.Login
                };
        }

        public static IList<TTo> MapAll<TFrom, TTo>(IList<TFrom> list, Func<TFrom, TTo> converter)
        {
            return list?.Select(converter).ToList();
        }
    }
}
