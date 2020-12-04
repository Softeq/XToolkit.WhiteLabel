// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.Common.Threading;

namespace Playground.Forms.Remote.ViewModels
{
    public class WorkItemViewModel : ObservableObject
    {
        private readonly Func<Action<string>, CancellationToken, Task> _operation;

        private string _name;
        private bool _isBusy;
        private string _resultData = string.Empty;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public WorkItemViewModel(
            string name,
            Func<Action<string>, CancellationToken, Task> operation,
            CancellationToken parentCancellationToken)
        {
            _operation = operation;
            _name = name;

            parentCancellationToken.Register(Cancel);

            RunCommand = new AsyncCommand(Request, () => !IsBusy);
            CancelCommand = new RelayCommand(Cancel, () => IsBusy);
        }

        public AsyncCommand RunCommand { get; }
        public RelayCommand CancelCommand { get; }

        public string Name
        {
            get => _name;
            private set => Set(ref _name, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                if (Set(ref _isBusy, value))
                {
                    RunCommand.RaiseCanExecuteChanged();
                    CancelCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string ResultData
        {
            get => _resultData;
            private set => Set(ref _resultData, value);
        }

        private async Task Request()
        {
            Cancel();

            IsBusy = true;

            try
            {
                await _operation(
                    message => Execute.BeginOnUIThread(() => ResultData = message), // operation log
                    _cts.Token);
            }
            catch (Exception ex)
            {
                ResultData = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void Cancel()
        {
            Interlocked.Exchange(ref _cts, new CancellationTokenSource()).Cancel();
        }
    }
}
