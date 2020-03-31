// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.Extended;
using Playground.Models;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.Pages
{
    public class DetailsPageViewModel : ViewModelBase
    {
        private readonly IExtendedDialogsService _dialogsService;

        public DetailsPageViewModel(
            IExtendedDialogsService dialogsService)
        {
            _dialogsService = dialogsService;
        }

        public Person? Person { get; set; }

        public override void OnAppearing()
        {
            base.OnAppearing();

            var title = Person?.FullName ?? string.Empty;
            var message = Person == null ? "You navigated without parameter" : "You navigated with parameter:";

            _dialogsService.ShowDialogAsync(new AlertDialogConfig(title, message, "OK"));
        }
    }
}
