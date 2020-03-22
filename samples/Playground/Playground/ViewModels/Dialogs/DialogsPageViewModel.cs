// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using System.Windows.Input;
using Playground.Converters;
using Playground.Models;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Dialogs
{
    public class DialogsPageViewModel : ViewModelBase
    {
        private readonly IDialogsService _dialogsService;
        private readonly ILogger _logger;

        private string _alertResult = "-";
        private Person? _dialogUntilDismissResult;
        private Person? _dialogUntilResult;

        public DialogsPageViewModel(
            IDialogsService dialogsService,
            ILogManager logManager)
        {
            _dialogsService = dialogsService;
            _logger = logManager.GetLogger<DialogsPageViewModel>();

            PersonConverter = new PersonToStringConverter();

            OpenAlertCommand = new AsyncCommand(OpenAlert);
            OpenDialogUntilDismissCommand = new RelayCommand(OpenDialogUntilDismiss);
            OpenDialogUntilResultCommand = new AsyncCommand(OpenDialogUntilResult);
            OpenTwoDialogsCommand = new AsyncCommand(OpenTwoDialogs);
        }

        public ICommand OpenAlertCommand { get; }

        public ICommand OpenDialogUntilDismissCommand { get; }

        public ICommand OpenDialogUntilResultCommand { get; }

        public ICommand OpenTwoDialogsCommand { get; }

        public string AlertResult
        {
            get => _alertResult;
            set => Set(ref _alertResult, value);
        }

        public Person DialogUntilDismissResult
        {
            get => _dialogUntilDismissResult!;
            set => Set(ref _dialogUntilDismissResult, value);
        }

        public Person DialogUntilResult
        {
            get => _dialogUntilResult!;
            set => Set(ref _dialogUntilResult, value);
        }

        public PersonToStringConverter PersonConverter { get; }

        private async Task OpenAlert()
        {
            var config = new ConfirmDialogConfig("~title", "~message", "~ok", "~cancel");
            var result = await _dialogsService.ShowDialogAsync(config);

            AlertResult = result.ToString();
        }

        private void OpenDialogUntilDismiss()
        {
            // simulate open from another thread

            Task.Run(() =>
            {
                _dialogsService
                    .For<SimpleDialogPageViewModel>()
                    .WithParam(x => x.FirstName, "First")
                    .WithParam(x => x.LastName, "Last")
                    .Navigate<Person>()
                    .ContinueOnUIThread(result =>
                    {
                        DialogUntilDismissResult = result;
                    });
            });
        }

        private async Task OpenDialogUntilResult()
        {
            var result = await _dialogsService
                .For<SimpleDialogPageViewModel>()
                .WithParam(x => x.FirstName, "First")
                .WithParam(x => x.LastName, "Last")
                .WithAwaitResult() // wait until result
                .Navigate<Person>();

            DialogUntilResult = result;
        }

        private async Task OpenTwoDialogs()
        {
            // simulate queue of dialogs

            var result1 = await _dialogsService
                .For<SimpleDialogPageViewModel>()
                .WithParam(x => x.FirstName, "First1")
                .WithParam(x => x.LastName, "Last1")
                .Navigate<Person>();

            _logger.Debug($"Dialog1 result: {PersonConverter.ConvertValue(result1)}");

            var result2 = await _dialogsService
               .For<SimpleDialogPageViewModel>()
               .WithParam(x => x.FirstName, $"{result1?.FirstName}2")
               .WithParam(x => x.LastName, $"{result1?.LastName}2")
               .Navigate<Person>();

            _logger.Debug($"Dialog2 result: {PersonConverter.ConvertValue(result2)}");
        }

        // TODO YP: add sample with custom dialog UI
    }
}
