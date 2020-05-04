// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Playground.Forms.ViewModels.Components
{
    [SuppressMessage("ReSharper", "ArgumentsStyleNamedExpression", Justification = "Used for sample")]
    [SuppressMessage("ReSharper", "ArgumentsStyleAnonymousFunction", Justification = "Used for sample")]
    public class AsyncCommandsPageViewModel : ViewModelBase
    {
        private readonly IDialogsService _dialogsService;

        private CancellationTokenSource _cts = new CancellationTokenSource();
        private bool _inProgress;

        public AsyncCommandsPageViewModel(
            IDialogsService dialogsService)
        {
            _dialogsService = dialogsService;

            WorkCommand = new AsyncCommand<int>(
                execute: DoWork,
                canExecute: _ => !InProgress,
                onException: ErrorHandler);

            CancelCommand = new AsyncCommand(
                execute: Cancel,
                canExecute: () => InProgress,
                onException: ErrorHandler);
        }

        public bool InProgress
        {
            get => _inProgress;
            set
            {
                if (Set(ref _inProgress, value))
                {
                    WorkCommand.RaiseCanExecuteChanged();
                    CancelCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public AsyncCommand<int> WorkCommand { get; }

        public AsyncCommand CancelCommand { get; }

        private async Task DoWork(int value)
        {
            InProgress = true;

            try
            {
                await Task.Delay(value * 1000, _cts.Token);
            }
            finally
            {
                InProgress = false;
            }
        }

        private Task Cancel()
        {
            Interlocked.Exchange(ref _cts, new CancellationTokenSource()).Cancel();

            return Task.CompletedTask;
        }

        private async void ErrorHandler(Exception ex)
        {
            await Execute.OnUIThreadAsync(async () =>
            {
                await _dialogsService.ShowDialogAsync(
                    new AlertDialogConfig("Exception", ex.Message, "OK"));
            });
        }
    }
}
