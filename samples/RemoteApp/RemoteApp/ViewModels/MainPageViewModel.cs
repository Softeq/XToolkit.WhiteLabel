using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.Remote;

namespace RemoteApp.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        private readonly DataService _dataService;

        private bool _isBusy;
        private string _resultData;
        private string _logData;

        private CancellationTokenSource _cts = new CancellationTokenSource();

        public MainPageViewModel()
        {
            // TODO: only for sample
            var logger = new DelegatingLogger();
            logger.Written += Logger_Written;
            _dataService = new DataService(new RemoteServiceFactory(), logger);

            RequestCommand = new AsyncCommand(Request);
            CancelRequestCommand = new RelayCommand(() => _cts.Cancel());
            ClearLogCommand = new RelayCommand(() => LogData = string.Empty);
        }

        public ICommand RequestCommand { get; }

        public ICommand CancelRequestCommand { get; }

        public ICommand ClearLogCommand { get; }

        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }

        public string ResultData
        {
            get => _resultData;
            set => Set(ref _resultData, value);
        }

        public string LogData
        {
            get => _logData;
            set => Set(ref _logData, value);
        }

        private async Task Request()
        {
            IsBusy = true;

            Interlocked.Exchange(ref _cts, new CancellationTokenSource()).Cancel();

            ResultData = await _dataService.GetDataAsync(_cts.Token);

            IsBusy = false;
        }

        private void Logger_Written(string message)
        {
            LogData += message + "\n";
        }
    }
}
