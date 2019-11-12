using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using RemoteServices.Auth;
using RemoteServices.Auth.Models;
using RemoteServices.Photos;
using RemoteServices.Profile;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.Remote;

namespace NetworkApp.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        private readonly DataService _dataService;
        private readonly IAuthService _authService;
        private readonly ProfileService _profileService;

        private bool _isBusy;
        private string _resultData;
        private string _logData;

        private CancellationTokenSource _cts = new CancellationTokenSource();

        private string _authUrl;
        private string _profileUrl;
        private string _clientId;
        private string _clientSecret;
        private string _login;
        private string _password;

        public MainPageViewModel()
        {
            // TODO: only for sample
            var logger = new DelegatingLogger();
            logger.Written += Logger_Written;
            _dataService = new DataService(new RemoteServiceFactory(), logger);

            if (string.IsNullOrEmpty(_authUrl))
            {
                throw new ArgumentNullException("Please add API config");
            }

            var tokenManager = new InMemoryTokenManager();

            _authService = new AuthService(new AuthRemoteService(new RemoteServiceFactory(), new AuthConfig
            {
                BaseUrl = _authUrl,
                ClientId = _clientId,
                ClientSecret = _clientSecret
            }), logger, tokenManager);

            var sessionContext = new SessionContext(tokenManager, _authService);

            _profileService = new ProfileService(new ProfileRemoteService(new RemoteServiceFactory(), sessionContext, _profileUrl));

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

            // ResultData = await _dataService.GetDataAsync(_cts.Token);

            var result0 = await _authService.LoginAsync(_login, _password, _cts.Token);

            await Task.Delay(3000);

            // var result = await _authService.RefreshTokenAsync(_cts.Token);

            try
            {
                var result = await _profileService.GetInfoAsync(_cts.Token);

                ResultData = result.ToString();
            }
            catch (Exception e)
            {
                ResultData = e.ToString();
            }

            IsBusy = false;
        }

        private void Logger_Written(string message)
        {
            LogData += message + "\n";
        }
    }
}
