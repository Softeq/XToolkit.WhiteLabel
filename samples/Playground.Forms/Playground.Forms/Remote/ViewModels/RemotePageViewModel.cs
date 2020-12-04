// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Playground.RemoteData.Auth;
using Playground.RemoteData.GitHub;
using Playground.RemoteData.HttpBin;
using Playground.RemoteData.Profile;
using Playground.RemoteData.Test;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.Forms.Remote.ViewModels
{
    public class RemotePageViewModel : ViewModelBase
    {
        // YP: add credentials
        private const string Login = "";
        private const string Password = "";

        private readonly JsonPlaceholderRemoteService _jsonPlaceholderRemoteService;
        private readonly IAuthService _authService;
        private readonly ProfileService _profileService;
        private readonly SslTestRemoteService _sslService;
        private readonly GitHubRemoteService _githubService;
        private readonly HttpBinRemoteService _httpBinRemoteService;

        private CancellationTokenSource? _cts;

        public ObservableRangeCollection<WorkItemViewModel> WorkItems { get; }

        public RemotePageViewModel(
            HttpBinRemoteService httpBinRemoteService,
            JsonPlaceholderRemoteService jsonPlaceholderRemoteService,
            IAuthService authService,
            ProfileService profileService,
            SslTestRemoteService sslService,
            GitHubRemoteService githubService)
        {
            _httpBinRemoteService = httpBinRemoteService;
            _jsonPlaceholderRemoteService = jsonPlaceholderRemoteService;
            _authService = authService;
            _profileService = profileService;
            _sslService = sslService;
            _githubService = githubService;

            WorkItems = new ObservableRangeCollection<WorkItemViewModel>();

            RunAllCommand = new RelayCommand(() => WorkItems.Apply(x => x.RunCommand.Execute(null)));
            CancelAllCommand = new RelayCommand(() => WorkItems.Apply(x => x.CancelCommand.Execute(null)));
        }

        public ICommand RunAllCommand { get; }

        public ICommand CancelAllCommand { get; }

        public override void OnAppearing()
        {
            base.OnAppearing();

            Initialize();
        }

        private void Initialize()
        {
            WorkItems.AddDuplicates(5, _ => new WorkItemViewModel(DataRequest, "Simple Data"));
            WorkItems.AddDuplicates(3, _ => new WorkItemViewModel(HttpBinCheckStatusRequest, "Http Status"));
            WorkItems.AddDuplicates(3, _ => new WorkItemViewModel(LoginRequest, "Login"));
            WorkItems.AddDuplicates(10, _ => new WorkItemViewModel(ProfileInfoRequest, "Profile Info"));
            WorkItems.AddDuplicates(3, _ => new WorkItemViewModel(ExpiredSslRequest, "Expired Ssl"));
            WorkItems.AddDuplicates(10, i => new WorkItemViewModel(GithubGetUserRequest, $"#{i.ToString()} GitHub User"));
            WorkItems.AddDuplicates(1, i => new WorkItemViewModel(FakeRequest, $"#{i.ToString()} fake"));
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            _cts?.Cancel();
        }

        private async Task FakeRequest(Action<string> callback, CancellationToken ct)
        {
            callback("start");

            await Task.Delay(2000, ct);

            callback("end");
        }

        private async Task DataRequest(Action<string> callback, CancellationToken ct)
        {
            callback("start");

            var result = await _jsonPlaceholderRemoteService.GetDataAsync(ct);

            callback($"end - {result}");
        }

        private async Task HttpBinCheckStatusRequest(Action<string> callback, CancellationToken ct)
        {
            callback("start");

            await _httpBinRemoteService.CheckStatusCodeAsync(200, ct);

            callback("end - done");
        }

        private async Task LoginRequest(Action<string> callback, CancellationToken ct)
        {
            callback("start");

            var result = await _authService.LoginAsync(Login, Password, ct);

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

            var result = await _sslService.CheckExpiredSslAsync(ct);

            callback($"end - {result}");
        }

        private async Task GithubGetUserRequest(Action<string> callback, CancellationToken ct)
        {
            callback("start");

            var user = await _githubService.GetUserAsync("wcoder", ct);

            callback(user == null
                ? "end - error - null"
                : $"end - {user.Name}");
        }
    }
}
