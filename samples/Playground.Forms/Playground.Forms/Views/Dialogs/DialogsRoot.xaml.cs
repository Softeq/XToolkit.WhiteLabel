// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.Forms.Views.Dialogs.Modal;
using Xamarin.Forms.Xaml;

namespace Playground.Forms.Views.Dialogs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DialogsRoot
    {
        public DialogsRoot()
        {
            InitializeComponent();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ModalPage());
        }
    }
}

