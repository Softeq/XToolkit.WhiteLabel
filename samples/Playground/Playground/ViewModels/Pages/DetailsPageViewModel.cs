// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.Models;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Pages
{
    public class DetailsPageViewModel : ViewModelBase
    {
        private readonly IDialogsService _dialogsService;

        public DetailsPageViewModel(
            IDialogsService dialogsService)
        {
            _dialogsService = dialogsService;
        }

        public Person? Person { get; set; }

        public override void OnAppearing()
        {
            base.OnAppearing();

            var title = Person?.FullName ?? string.Empty;
            var message = Person == null ? "You navigated without parameter" : "You navigated with parameter:";

            _dialogsService.ShowDialogAsync(title, message, "OK");
        }
    }
}