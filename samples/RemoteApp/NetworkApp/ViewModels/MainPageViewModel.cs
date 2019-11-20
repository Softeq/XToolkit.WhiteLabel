using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using RemoteServices.Auth;
using RemoteServices.Auth.Models;
using RemoteServices.Photos;
using RemoteServices.Profile;
using RemoteServices.Tests;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote;

namespace NetworkApp.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        private readonly DataService _dataService;
        private readonly IAuthService _authService;
        private readonly ProfileService _profileService;
        private readonly TestRemoteService _testService;
        private readonly NewDataService _newDataService;

        public ObservableRangeCollection<WorkItemViewModel> WorkItems { get; } = new ObservableRangeCollection<WorkItemViewModel>();

        private string _authUrl;
        private string _profileUrl;
        private string _clientId;
        private string _clientSecret;
        private string _login;
        private string _password;

        public MainPageViewModel(
            NewDataService newDataService
            )
        {
            _newDataService = newDataService;


            // init services
            var logger = new ConsoleLogger("NetworkApp");

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
            }, logger), logger, tokenManager);

            var sessionContext = new SessionContext(tokenManager, _authService);

            _profileService = new ProfileService(new ProfileRemoteService(new RemoteServiceFactory(), sessionContext, _profileUrl, logger));

            _testService = new TestRemoteService(logger);



            // fill list
            for (int i = 0; i < 5; i++)
            {
                WorkItems.Add(new WorkItemViewModel(DataRequest) { Name = $"Simple Data" });
            }

            //WorkItems.Add(new WorkItemViewModel(LoginRequest) { Name = $"Login" });

            //for (int i = 0; i < 10; i++)
            //{
            //    WorkItems.Add(new WorkItemViewModel(ProfileInfoRequest) { Name = $"Profile Info" });
            //}

            //WorkItems.Add(new WorkItemViewModel(ExpiredSslRequest) { Name = $"Expired Ssl" });

            //for (int i = 0; i < 100; i++)
            //{
            //    WorkItems.Add(new WorkItemViewModel(FakeRequest) { Name = $"#{i} fake" });
            //}

            RunAllCommand = new RelayCommand(() => WorkItems.Apply(x => x.RunCommand.Execute(null)));
            CancelAllCommand = new RelayCommand(() => WorkItems.Apply(x => x.CancelCommand.Execute(null)));
        }

        public ICommand RunAllCommand { get; }

        public ICommand CancelAllCommand { get; }

        private async Task FakeRequest(Action<string> callback, CancellationToken ct)
        {
            callback("start");

            await Task.Delay(2000, ct);

            callback("end");
        }

        private async Task DataRequest(Action<string> callback, CancellationToken ct)
        {
            callback("start");

            //var result = await _dataService.GetDataAsync(ct);
            await _newDataService.GetAllPhotosAsync(ct);
            var result = "done";

            callback($"end - {result}");
        }

        private async Task LoginRequest(Action<string> callback, CancellationToken ct)
        {
            callback("start");

            var result = await _authService.LoginAsync(_login, _password, ct);

            callback($"end - {result}");
        }

        private async Task ProfileInfoRequest(Action<string> callback, CancellationToken ct)
        {
            callback("start");

            await Task.Delay(1000);

            var result = await _profileService.GetInfoAsync(ct);

            callback($"end - {result}");
        }

        private async Task ExpiredSslRequest(Action<string> callback, CancellationToken ct)
        {
            callback("start");

            var result = await _testService.CheckExpiredSslAsync(ct);

            callback($"end - {result}");
        }
    }
}
