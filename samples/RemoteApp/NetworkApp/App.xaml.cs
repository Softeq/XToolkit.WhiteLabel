// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using System.Net.Http;
using Xamarin.Forms;
using DryIoc;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote.Auth;
using Softeq.XToolkit.Remote.Client;
using RemoteServices.Auth;
using RemoteServices.Auth.Models;
using RemoteServices.Profile;
using RemoteServices.Profile.Models;
using RemoteServices.GitHub;
using RemoteServices.Test;
using NetworkApp.Pages;
using NetworkApp.ViewModels;

namespace NetworkApp
{
    public partial class App : Application
    {
        private string _authUrl;
        private string _clientId;
        private string _clientSecret;
        private string _profileUrl;
        private string _githubPersonalToken;

        private readonly IContainer _container;

        public App(HttpMessageHandler customHttpMessageHandler) // sample of using custom primary http message handler
        {
            // YP: currently not working for AndroidClientHandler (https://github.com/xamarin/xamarin-android/issues/3682)
            //ServicePointManager.ServerCertificateValidationCallback =
            //    (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) =>
            //    {
            //        return true;
            //    };

            InitializeComponent();




            // ==== Begin configuration

            _container = new Container(rules => rules.WithoutFastExpressionCompiler());

            // Common Services

            _container.Register<ILogManager, ConsoleLogManager>();

            // Remote Services

            _container.Register<IHttpClientFactory, DefaultHttpClientFactory>(Reuse.Singleton);
            _container.Register<IRemoteServiceFactory, RemoteServiceFactory>(Reuse.Singleton);

            // Auth Services

            _container.RegisterDelegate(r => new AuthConfig(_authUrl, _clientId, _clientSecret), Reuse.Singleton);
            _container.Register<IAuthRemoteService, AuthRemoteService>(Reuse.Singleton);
            _container.Register<ITokenManager, InMemoryTokenManager>(Reuse.Singleton);
            _container.Register<IAuthService, AuthService>(Reuse.Singleton);
            _container.Register<ISessionContext, SessionContext>(Reuse.Singleton);

            // Profile Services

            _container.RegisterDelegate(r => new ProfileConfig(_profileUrl), Reuse.Singleton);
            _container.Register<ProfileRemoteService>(Reuse.Singleton);
            _container.Register<ProfileService>(Reuse.Singleton);

            // GitHub Services

            _container.Register<GitHubRemoteService>(Reuse.Singleton);
            _container.RegisterDelegate<GitHubSessionContext>(r => new GitHubSessionContext(_githubPersonalToken), Reuse.Singleton);

            // Ssl Test Services

            _container.Register<SslTestRemoteService>(Reuse.Singleton);
            _container.RegisterDelegate<HttpMessageHandler>(r => customHttpMessageHandler, Reuse.Singleton);

            // Other Data Services

            _container.Register<HttpBinRemoteService>(Reuse.Singleton);
            _container.Register<JsonPlaceholderRemoteService>(Reuse.Singleton);

            // ViewModels

            _container.Register<MainPageViewModel>(Reuse.Transient);

            // ==== End configuration

            _container.Validate();


            var mainPage = new MainPage
            {
                BindingContext = _container.Resolve<MainPageViewModel>()
            };

            MainPage = new NavigationPage(mainPage);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

    }
}
