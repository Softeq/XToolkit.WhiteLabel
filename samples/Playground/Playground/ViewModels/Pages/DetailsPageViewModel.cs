// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Playground.Models;

namespace Playground.ViewModels.Pages
{
    public class DetailsPageViewModel : ViewModelBase
    {
        private readonly IDialogsService _dialogsService;

        private Person _person;

        public DetailsPageViewModel(
            IDialogsService dialogsService)
        {
            _dialogsService = dialogsService;
        }

        public Person Parameter
        {
            get => null;
            set => _person = value;
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            var title = _person == null ? string.Empty : $"{_person.FirstName} {_person.LastName}";
            var message = _person == null ? "You navigated without parameter" : "You navigated with parameter:";

            _dialogsService.ShowDialogAsync(title, message, "ok");
        }
    }
}