namespace RemoteApp.Services.Auth.Dtos
{
    internal static class AuthConsts
    {
        public const string GrantTypePassword = "password";
        public const string GrantTypeRefreshToken = "refresh_token";

        public const string ScopeApi = "api";
        public const string ScopeOfflineAccess = "offline_access";
        public const string ScopeApiOfflineAccess = ScopeApi + " " + ScopeOfflineAccess;
    }
}
