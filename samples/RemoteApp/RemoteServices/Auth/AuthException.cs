// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using RemoteServices.Auth.Models;

namespace RemoteServices.Auth
{
    public class AuthException : Exception
    {
        public AuthException(ErrorResult error, Exception ex) : base("API Error", ex)
        {
            Error = error;
        }

        public ErrorResult Error { get; }
    }
}
