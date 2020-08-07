// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using Playground.Converters;
using Playground.Extended;
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
        private readonly IExtendedDialogsService _dialogsService;
        private readonly ILogger _logger;

        private string _alertResult = "-";
        private string _confirmResult = "-";
        private string _actionSheetResult = "-";
        private Person? _dialogUntilDismissResult;
        private Person? _dialogUntilResult;

        public DialogsPageViewModel(
            IExtendedDialogsService dialogsService,
            ILogManager logManager)
        {
            _dialogsService = dialogsService;
            _logger = logManager.GetLogger<DialogsPageViewModel>();

            PersonConverter = new PersonToStringConverter();

            OpenAlertCommand = new AsyncCommand(OpenAlert);
            OpenConfirmCommand = new AsyncCommand(OpenConfirm);
            OpenActionSheetCommand = new AsyncCommand(OpenActionSheet);
            OpenDialogUntilDismissCommand = new RelayCommand(OpenDialogUntilDismiss);
            OpenDialogUntilResultCommand = new AsyncCommand(OpenDialogUntilResult);
            OpenTwoDialogsCommand = new AsyncCommand(OpenTwoDialogs);
        }

        public ICommand OpenAlertCommand { get; }

        public ICommand OpenConfirmCommand { get; }

        public ICommand OpenActionSheetCommand { get; }

        public ICommand OpenDialogUntilDismissCommand { get; }

        public ICommand OpenDialogUntilResultCommand { get; }

        public ICommand OpenTwoDialogsCommand { get; }

        public string AlertResult
        {
            get => _alertResult;
            private set => Set(ref _alertResult, value);
        }

        public string ConfirmResult
        {
            get => _confirmResult;
            private set => Set(ref _confirmResult, value);
        }

        public string ActionSheetResult
        {
            get => _actionSheetResult;
            private set => Set(ref _actionSheetResult, value);
        }

        public Person DialogUntilDismissResult
        {
            get => _dialogUntilDismissResult!;
            private set => Set(ref _dialogUntilDismissResult, value);
        }

        public Person DialogUntilResult
        {
            get => _dialogUntilResult!;
            private set => Set(ref _dialogUntilResult, value);
        }

        public PersonToStringConverter PersonConverter { get; }

        private async Task OpenAlert()
        {
            await _dialogsService.ShowDialogAsync(new AlertDialogConfig("~title", "~message", "~ok"));

            var config = new ChooseBetterDateDialogConfig
            {
                Title = "Please choose the date:",
                First = DateTime.Now.AddDays(1),
                Second = DateTime.Now.AddDays(4)
            };
            var result = await _dialogsService.ShowDialogAsync(config);

            AlertResult = result.ToString(CultureInfo.CurrentCulture);
        }

        private async Task OpenConfirm()
        {
            var config = new ConfirmDialogConfig("~title - remove?", "~message", "~yes", "~no")
            {
                IsDestructive = true
            };
            var result = await _dialogsService.ShowDialogAsync(config);

            ConfirmResult = result ? "Removed" : "Declined";
        }

        private async Task OpenActionSheet()
        {
            var config = new ActionSheetDialogConfig
            {
                Title = "~title",
                OptionButtons = new[]
                {
                    "~option 1",
                    "~option 2",
                    "~option 3"
                },
                CancelButtonText = "~cancel",
                DestructButtonText = "~destruct"
            };
            var result = await _dialogsService.ShowDialogAsync(config);

            ActionSheetResult = result;
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
