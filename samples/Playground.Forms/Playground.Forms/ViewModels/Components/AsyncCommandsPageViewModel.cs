// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.Forms.ViewModels.Components
{
    public class AsyncCommandsPageViewModel : ViewModelBase
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private bool _inProgress;

        public AsyncCommandsPageViewModel()
        {
            WorkCommand = new AsyncCommand<int>(
                action: DoWork,
                canExecute: () => !InProgress);

            CancelCommand = new AsyncCommand(
                action: Cancel,
                canExecute: () => InProgress);
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
            catch (TaskCanceledException)
            {
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
    }
}
