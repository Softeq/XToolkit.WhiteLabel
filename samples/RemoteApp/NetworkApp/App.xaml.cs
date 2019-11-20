using Xamarin.Forms;
using NetworkApp.Pages;
using NetworkApp.ViewModels;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using RemoteServices.Photos;

namespace NetworkApp
{
    public partial class App : Application
    {
        public App()
        {
            // YP: currently not working for AndroidClientHandler (https://github.com/xamarin/xamarin-android/issues/3682)
            ServicePointManager.ServerCertificateValidationCallback =
                (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) =>
                {
                    return true;
                };



            IHttpClientFactory httpClientFactory = new DefaultHttpClientFactory();
            IApiServiceFactory apiServiceFactory = new RefitApiServiceFactory();

            SimpleIoc.Register<Lazy<IPhotosApiService>>(new Lazy<IPhotosApiService>(() =>
            {
                var httpClient = httpClientFactory.CreateHttpClient("https://jsonplaceholder.typicode.com");

                var apiService = apiServiceFactory.CreateService<IPhotosApiService>(httpClient);

                return apiService;
            }));




            InitializeComponent();

            var mainPage = new MainPage
            {
                BindingContext = SimpleIoc.Resolve<MainPageViewModel>()
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
