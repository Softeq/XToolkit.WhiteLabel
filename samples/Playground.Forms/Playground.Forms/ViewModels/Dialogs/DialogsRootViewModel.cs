// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using System.Windows.Input;
using Playground.Forms.ViewModels.Dialogs.Modal;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels.Dialogs
{
    public class DialogsRootViewModel : ViewModelBase
    {
        private readonly IDialogsService _dialogsService;

        private string _dialogResult = string.Empty;

        public DialogsRootViewModel(
            IDialogsService dialogsService)
        {
            _dialogsService = dialogsService;

            ShowModalScreenCommand = new AsyncCommand(ShowModalScreenAsync);
            ShowModalScreenWithoutWaitCommand = new AsyncCommand(ShowModalScreenWithoutWait);
            AlertCommand = new AsyncCommand(AlertAsync);
            AlertWithTwoButtonsCommand = new AsyncCommand(AlertWithTwoButtonsAsync);
        }

        public ICommand ShowModalScreenCommand { get; }

        public ICommand ShowModalScreenWithoutWaitCommand { get; }

        public ICommand AlertCommand { get; }

        public ICommand AlertWithTwoButtonsCommand { get; }

        public string DialogResult
        {
            get => _dialogResult;
            set => Set(ref _dialogResult, value);
        }

        private async Task ShowModalScreenAsync()
        {
            var result = await _dialogsService
                .For<ModalPageViewModel>()
                .WithParam(x => x.Title, "With")
                .WithParam(x => x.Message, "Wait")
                .Navigate<ModalPageDialogResult>();

            DialogResult = result == null
                ? "result: null"
                : $"result: {result.Text}";
        }

        private async Task ShowModalScreenWithoutWait()
        {
            var result = await _dialogsService
                .For<ModalPageViewModel>()
                .WithParam(x => x.Title, "Without")
                .WithParam(x => x.Message, "Wait")
                .WithAwaitResult()
                .Navigate<ModalPageDialogResult>();

            DialogResult = result == null
                ? "result: null"
                : $"result: {result.Text}";
        }

        private Task AlertAsync()
        {
            return _dialogsService.ShowDialogAsync("simple title", "simple message", "ok");
        }

        private async Task AlertWithTwoButtonsAsync()
        {
            var result = await _dialogsService.ShowDialogAsync("simple title", "simple message", "ok", "cancel");

            DialogResult = $"result: {result}";
        }
    }
}
