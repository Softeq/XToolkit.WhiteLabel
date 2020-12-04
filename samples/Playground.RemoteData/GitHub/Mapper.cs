// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Playground.RemoteData.GitHub.Dtos;
using Playground.RemoteData.GitHub.Models;

namespace Playground.RemoteData.GitHub
{
    public static class Mapper
    {
        public static User Map(UserDetailsResponse? x)
        {
            return x == null
                ? new User()
                : new User
                {
                    Name = x.Name
                };
        }

        public static User Map(UserResponse? x)
        {
            return x == null
                ? new User()
                : new User
                {
                    Name = x.Login
                };
        }

        public static IList<TTo> MapAll<TFrom, TTo>(IList<TFrom>? list, Func<TFrom, TTo> converter)
        {
            return list?.Select(converter).ToList()!;
        }
    }
}
