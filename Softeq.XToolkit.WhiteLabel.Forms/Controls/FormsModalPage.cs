// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq;
using ImTools;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Softeq.XToolkit.WhiteLabel.Forms.Controls
{
    public abstract class FormsModalPage : ContentPage
    {
        protected FormsModalPage()
        {
            On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FormSheet);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (Device.RuntimePlatform == Device.iOS)
            {
                var lastItemInStack = Navigation.ModalStack.LastOrDefault();
                if (lastItemInStack == this)
                {
                    RaiseCloseCommand();
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            var result = base.OnBackButtonPressed();

            if (Device.RuntimePlatform == Device.Android)
            {
                RaiseCloseCommand();
            }

            return result;
        }

        private void RaiseCloseCommand()
        {
            if (BindingContext is IDialogViewModel dialogViewModel)
            {
                dialogViewModel.DialogComponent.CloseCommand.Execute(null);
            }
        }
    }
}
