// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FFImageLoading;
using FFImageLoading.Cache;
using Playground.Forms.Remote.ViewModels;
using Playground.RemoteData.Auth;
using Playground.RemoteData.GitHub;
using Playground.RemoteData.HttpBin;
using Playground.RemoteData.Profile;
using Playground.RemoteData.Test;

namespace Playground.Forms.Remote.Services
{
    public class RemoteDataService
    {
        // YP: add credentials
        private const string AuthLogin = "";
        private const string AuthPassword = "";

        private readonly JsonPlaceholderRemoteService _jsonPlaceholderRemoteService;
        private readonly IAuthService _authService;
        private readonly ProfileService _profileService;
        private readonly SslTestRemoteService _sslService;
        private readonly GitHubRemoteService _githubService;
        private readonly HttpBinRemoteService _httpBinRemoteService;

        public RemoteDataService(
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
        }

        public IEnumerable<WorkItemViewModel> GetItems(CancellationToken token)
        {
            var items = new List<(int, Func<int, WorkItemViewModel>)>
            {
                (5, _ => new WorkItemViewModel("Simple Data", DataRequest, token)),
                (3, _ => new WorkItemViewModel("Http Status", HttpBinCheckStatusRequest, token)),
                (3, _ => new WorkItemViewModel("Login", LoginRequest, token)),
                (10, _ => new WorkItemViewModel("Profile Info", ProfileInfoRequest, token)),
                (3, _ => new WorkItemViewModel("Expired Ssl", ExpiredSslRequest, token)),
                (10, i => new WorkItemViewModel($"#{i.ToString()} GitHub User", GithubGetUserRequest, token)),
                (2, i => new WorkItemViewModel($"POST #{i.ToString()}", PostRequest, token)),
                (3, _ => new WorkItemViewModel("Upload Stream", UploadStreamRequest, token)),
                (2, _ => new WorkItemViewModel("Download Stream", DownloadStreamRequest, token)),
                (2, _ => new WorkItemViewModel("FFImageLoading Client", FFImageLoadingRequest, token))
            };

            foreach (var (count, factory) in items)
            {
                for (int i = 0; i < count; i++)
                {
                    yield return factory(i);
                }
            }
        }

        private async Task PostRequest(Action<string> callback, CancellationToken ct)
        {
            callback("started");

            await _httpBinRemoteService.PostFormAsync(ct).ConfigureAwait(false);

            callback("end");
        }

        private async Task DataRequest(Action<string> callback, CancellationToken ct)
        {
            callback("started");

            var result = await _jsonPlaceholderRemoteService.GetDataAsync(ct).ConfigureAwait(false);

            callback($"end - {result}");
        }

        private async Task HttpBinCheckStatusRequest(Action<string> callback, CancellationToken ct)
        {
            callback("started");

            await _httpBinRemoteService.StatusCodeAsync(200, ct).ConfigureAwait(false);

            callback("end - done");
        }

        private async Task LoginRequest(Action<string> callback, CancellationToken ct)
        {
            callback("started");

            var result = await _authService.LoginAsync(AuthLogin, AuthPassword, ct).ConfigureAwait(false);

            callback($"end - {result}");
        }

        private async Task ProfileInfoRequest(Action<string> callback, CancellationToken ct)
        {
            callback("started");

            await Task.Delay(1000);

            var result = await _profileService.GetInfoAsync(ct).ConfigureAwait(false);

            callback($"end - {result}");
        }

        private async Task ExpiredSslRequest(Action<string> callback, CancellationToken ct)
        {
            callback("started");

            var result = await _sslService.CheckExpiredSslAsync(ct).ConfigureAwait(false);

            callback($"end - {result}");
        }

        private async Task GithubGetUserRequest(Action<string> callback, CancellationToken ct)
        {
            callback("started");

            var user = await _githubService.GetUserAsync("wcoder", ct).ConfigureAwait(false);

            callback($"end - {user.Name}");
        }

        private async Task UploadStreamRequest(Action<string> callback, CancellationToken ct)
        {
            await Task.Run(async () =>
            {
                callback("started");

                await using var memoryStream = new MemoryStream();
                memoryStream.SetLength(2 * 1024 * 1024); // 2Mb

                var result = await _httpBinRemoteService.UploadAsync(memoryStream, ct).ConfigureAwait(false);

                callback(result);

                callback($"end");
            }).ConfigureAwait(false);
        }

        private async Task DownloadStreamRequest(Action<string> callback, CancellationToken ct)
        {
            callback("started");

            var result = await _httpBinRemoteService.GetStreamAsync(99, ct).ConfigureAwait(false);

            await foreach (var item in result.WithCancellation(ct))
            {
                callback(item.Url?.ToString() ?? string.Empty);
            }

            callback($"end");
        }

        private async Task FFImageLoadingRequest(Action<string> callback, CancellationToken ct)
        {
            callback("started");

            var random = new Random().NextDouble();

            await ImageService.Instance.LoadUrl($"https://picsum.photos/1000?rand={random}")
                .DownloadProgress(progress => callback($"{progress.Current/progress.Total}..."))
                .Error(ex => callback(ex.Message))
                .Finish(x => callback("finished"))
                .WithCache(CacheType.None)
                .PreloadAsync();
        }
    }
}
