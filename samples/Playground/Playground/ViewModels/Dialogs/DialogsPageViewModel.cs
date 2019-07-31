// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using System.Windows.Input;
using Playground.Models;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Dialogs
{
    public class DialogsPageViewModel : ViewModelBase
    {
        private readonly IDialogsService _dialogsService;

        private Person _dialogUntilDismissResult;
        private string _alertResult;

        public DialogsPageViewModel(
            IDialogsService dialogsService)
        {
            _dialogsService = dialogsService;

            OpenDialogUntilDismissCommand = new AsyncCommand(OpenDialogUntilDismiss);
            OpenAlertCommand = new AsyncCommand(OpenAlert);
        }

        public ICommand OpenDialogUntilDismissCommand { get; }

        public ICommand OpenAlertCommand { get; }

        public Person DialogUntilDismissResult
        {
            get => _dialogUntilDismissResult;
            set => Set(ref _dialogUntilDismissResult, value);
        }

        public string AlertResult
        {
            get => _alertResult;
            set => Set(ref _alertResult, value);
        }

        private async Task OpenDialogUntilDismiss()
        {
            var result = await _dialogsService
                .For<DialogPageViewModel>()
                .WithParam(x => x.FirstName, "First")
                .WithParam(x => x.LastName, "Last")
                .Navigate<Person>();

            DialogUntilDismissResult = result;
        }

        private async Task OpenAlert()
        {
            var result = await _dialogsService.ShowDialogAsync("~title", "~message", "~ok", "~cancel");

            AlertResult = result.ToString();
        }
    }
}
