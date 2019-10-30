using System;
using RemoteApp.Services.Auth.Models;

namespace RemoteApp.Services.Auth
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