using Xamarin.Forms;
using NetworkApp.Pages;
using NetworkApp.ViewModels;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace NetworkApp
{
    public partial class App : Application
    {
        public App()
        {
            // YP: not working for AndroidClientHandler
            // More details: https://thomasbandt.com/certificate-and-public-key-pinning-with-xamarin
            ServicePointManager.ServerCertificateValidationCallback = SslCertificateValidationCallback;

            InitializeComponent();

            var mainPage = new MainPage
            {
                BindingContext = new MainPageViewModel()
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

        private bool SslCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
