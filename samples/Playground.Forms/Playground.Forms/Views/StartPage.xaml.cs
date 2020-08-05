// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Playground.Forms.Views
{
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
