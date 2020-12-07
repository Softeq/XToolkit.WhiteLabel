// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.Forms.Remote.Services;
using Playground.Forms.Remote.ViewModels;
using Playground.RemoteData.Auth;
using Playground.RemoteData.Auth.Models;
using Playground.RemoteData.GitHub;
using Playground.RemoteData.HttpBin;
using Playground.RemoteData.Profile;
using Playground.RemoteData.Profile.Models;
using Playground.RemoteData.Test;
using Refit;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Api;
using Softeq.XToolkit.Remote.Auth;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;

namespace Playground.Forms.Remote
{
    public static class RemoteBootstrapper
    {
        // YP: add credentials
        private const string AuthApiUrl = "";
        private const string AuthClientId = "";
        private const string AuthClientSecret = "";
        private const string ProfileApiUrl = "";
        private const string GithubPersonalToken = "";

        public static void ConfigureIoc(IContainerBuilder builder)
        {
            // XToolkit.Remote

            // Basic usage
            builder.Singleton<DefaultHttpClientFactory, IHttpClientFactory>();
            builder.Singleton<RemoteServiceFactory, IRemoteServiceFactory>();

            // Optional: Use custom content serializer settings
            //           Explicit ApiServiceFactory registration to use the new configuration
            builder.Singleton<RefitApiServiceFactory, IApiServiceFactory>();
            builder.Singleton(_ => new RefitSettings
            {
                // JsonSerializerSettings object also can be used via IoC
                ContentSerializer = new NewtonsoftJsonContentSerializer(
                    Softeq.XToolkit.WhiteLabel.Services.JsonSerializer.DefaultSettings)
            });

            // Auth API

            builder.Singleton(_ => new AuthApiConfig(AuthApiUrl, AuthClientId, AuthClientSecret));
            builder.Singleton<AuthRemoteService, IAuthRemoteService>();
            builder.Singleton<InMemoryTokenManager, ITokenManager>();
            builder.Singleton<AuthService, IAuthService>();
            builder.Singleton<SessionContext, ISessionContext>();

            // Profile API

            builder.Singleton(_ => new ProfileApiConfig(ProfileApiUrl));
            builder.Singleton<ProfileRemoteService>();
            builder.Singleton<ProfileService>();

            // GitHub API

            builder.Singleton(_ => new GitHubSessionContext(GithubPersonalToken));
            builder.Singleton<GitHubRemoteService>();

            // Ssl API

            builder.Singleton<SslTestRemoteService>();

            // Http Bin API

            builder.Singleton<HttpBinRemoteService>();

            // JsonPlaceholder API

            builder.Singleton<JsonPlaceholderRemoteService>();

            // ViewModels
            builder.PerDependency<RemotePageViewModel>();

            // Data Services
            builder.Singleton<RemoteDataService>();
        }
    }
}
