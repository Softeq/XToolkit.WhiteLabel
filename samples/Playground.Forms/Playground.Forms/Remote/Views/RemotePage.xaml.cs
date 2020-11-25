// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.Forms.Remote.ViewModels;

namespace Playground.Forms.Remote.Views
{
    public partial class RemotePage
    {
        public RemotePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is RemotePageViewModel vm)
            {
                vm.OnAppearing();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (BindingContext is RemotePageViewModel vm)
            {
                vm.OnDisappearing();
            }
        }
    }
}

