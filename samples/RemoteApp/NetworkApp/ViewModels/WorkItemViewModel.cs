// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Commands;

namespace NetworkApp.ViewModels
{
    public class WorkItemViewModel : ObservableObject
    {
        private readonly Func<Action<string>, CancellationToken, Task> _operation;

        private string _name;
        private bool _isBusy;
        private string _resultData = string.Empty;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public WorkItemViewModel(
            Func<Action<string>, CancellationToken, Task> operation,
            string name)
        {
            _operation = operation;
            _name = name;

            RunCommand = new AsyncCommand(Request);
            CancelCommand = new RelayCommand(Cancel);
        }

        public ICommand RunCommand { get; }
        public ICommand CancelCommand { get; }

        public string Name
        {
            get => _name;
            private set => Set(ref _name, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            private set => Set(ref _isBusy, value);
        }

        public string ResultData
        {
            get => _resultData;
            private set => Set(ref _resultData, value);
        }

        private async Task Request()
        {
            Interlocked.Exchange(ref _cts, new CancellationTokenSource()).Cancel();

            IsBusy = true;

            try
            {
                await _operation(
                    message => ResultData = message, // operation log
                    _cts.Token);
            }
            catch (Exception ex)
            {
                ResultData = ex.Message;
            }

            IsBusy = false;
        }

        private void Cancel()
        {
            _cts.Cancel();
        }
    }
}
