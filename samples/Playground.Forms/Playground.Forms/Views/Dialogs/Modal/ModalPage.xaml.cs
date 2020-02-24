// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Xamarin.Forms.Xaml;

namespace Playground.Forms.Views.Dialogs.Modal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModalPage
    {
        public ModalPage()
        {
            InitializeComponent();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void Button_OnClicked1(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SecondModalPage());
        }
    }
}
