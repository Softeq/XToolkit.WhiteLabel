using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using RemoteServices.Auth;
using RemoteServices.Photos;
using RemoteServices.Profile;
using RemoteServices.Tests;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.Common.Extensions;

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

        private string _login;
        private string _password;

        public MainPageViewModel(
            NewDataService newDataService,
            DataService dataService,
            IAuthService authService,
            ProfileService profileService,
            TestRemoteService testRemoteService)
        {
            _newDataService = newDataService;
            _dataService = dataService;
            _authService = authService;
            _profileService = profileService;
            _testService = testRemoteService;



            // fill list
            for (int i = 0; i < 5; i++)
            {
                WorkItems.Add(new WorkItemViewModel(DataRequest) { Name = $"Simple Data" });
            }

            WorkItems.Add(new WorkItemViewModel(NewDataRequest) { Name = $"New Simple Data" });

            WorkItems.Add(new WorkItemViewModel(LoginRequest) { Name = $"Login" });

            for (int i = 0; i < 10; i++)
            {
                WorkItems.Add(new WorkItemViewModel(ProfileInfoRequest) { Name = $"Profile Info" });
            }

            WorkItems.Add(new WorkItemViewModel(ExpiredSslRequest) { Name = $"Expired Ssl" });

            for (int i = 0; i < 10; i++)
            {
                WorkItems.Add(new WorkItemViewModel(FakeRequest) { Name = $"#{i} fake" });
            }

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

            var result = await _dataService.GetDataAsync(ct);

            callback($"end - {result}");
        }

        private async Task NewDataRequest(Action<string> callback, CancellationToken ct)
        {
            callback("start");

            await _newDataService.GetAllPhotosAsync(ct);

            callback($"end - done");
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
