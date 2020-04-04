// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels.Dialogs.Modal
{
    public class ModalPageViewModel : DialogViewModelBase
    {
        private readonly IDialogsService _dialogsService;

        private string _text = string.Empty;

        public ModalPageViewModel(IDialogsService dialogsService)
        {
            _dialogsService = dialogsService;

            OpenDialogCommand = new AsyncCommand(OpenDialogAsync);
            SaveCommand = new RelayCommand(() =>
            {
                DialogComponent.CloseCommand.Execute(new ModalPageDialogResult(Text));
            });
        }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string Text
        {
            get => _text;
            set => Set(ref _text, value);
        }

        public ICommand OpenDialogCommand { get; }

        public ICommand SaveCommand { get; }

        private async Task OpenDialogAsync()
        {
            await _dialogsService
                .For<SecondModalPageViewModel>()
                .Navigate();

            Text = "second page closed";
        }
    }

    public class ModalPageDialogResult
    {
        public ModalPageDialogResult(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}
