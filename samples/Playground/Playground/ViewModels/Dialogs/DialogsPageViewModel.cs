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

        private string _alertResult = "-";
        private Person _dialogUntilDismissResult;

        public DialogsPageViewModel(
            IDialogsService dialogsService)
        {
            _dialogsService = dialogsService;

            OpenAlertCommand = new AsyncCommand(OpenAlert);
            OpenDialogUntilDismissCommand = new AsyncCommand(OpenDialogUntilDismiss);
        }

        public ICommand OpenAlertCommand { get; }

        public ICommand OpenDialogUntilDismissCommand { get; }

        public string AlertResult
        {
            get => _alertResult;
            set => Set(ref _alertResult, value);
        }

        public Person DialogUntilDismissResult
        {
            get => _dialogUntilDismissResult;
            set => Set(ref _dialogUntilDismissResult, value);
        }

        private async Task OpenAlert()
        {
            var result = await _dialogsService.ShowDialogAsync("~title", "~message", "~ok", "~cancel");

            AlertResult = result.ToString();
        }

        private async Task OpenDialogUntilDismiss()
        {
            var result = await _dialogsService
                .For<SimpleDialogPageViewModel>()
                .WithParam(x => x.FirstName, "First")
                .WithParam(x => x.LastName, "Last")
                .Navigate<Person>();

            DialogUntilDismissResult = result;
        }
    }
}
