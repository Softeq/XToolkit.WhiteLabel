// Developed by Softeq Development Corporation
// http://www.softeq.com

using Xamarin.Forms.Xaml;

namespace Playground.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage
    {
        public StartPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ActivityIndicator.IsRunning = true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ActivityIndicator.IsRunning = false;
        }
    }
}

