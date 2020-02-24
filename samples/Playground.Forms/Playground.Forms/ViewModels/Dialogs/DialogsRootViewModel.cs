// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics;
using System.Reflection;
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
        private string _dialogResult = String.Empty;

        public DialogsRootViewModel(
            IDialogsService dialogsService)
        {
            _dialogsService = dialogsService;
            ShowModalScreenCommand = new AsyncCommand(ShowModalScreenAsync);
            ShowModalScreenWithoutWaitCommand = new AsyncCommand(ShowModalScreenWithoutWait);
        }

        public ICommand ShowModalScreenCommand { get; }

        public ICommand ShowModalScreenWithoutWaitCommand { get; }

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
                .Navigate<ModalPageDialogResult>()
                .ConfigureAwait(false);

            DialogResult = result == null ? "result: null" : $"result: {result.Text}";
        }

        private async Task ShowModalScreenWithoutWait()
        {
            var result = await _dialogsService
                .For<ModalPageViewModel>()
                .WithParam(x => x.Title, "Without")
                .WithParam(x => x.Message, "Wait")
                .WithAwaitResult()
                .Navigate<ModalPageDialogResult>()
                .ConfigureAwait(false);

            DialogResult = result == null ? "result: null" : $"result: {result.Text}";
        }
    }
}
