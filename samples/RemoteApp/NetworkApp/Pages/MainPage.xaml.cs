using NetworkApp.ViewModels;
using Xamarin.Forms;

namespace NetworkApp.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is MainPageViewModel vm)
            {
                vm.Initialize();
            }
        }
    }
}
