// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Primitives;

namespace RemoteServices.Test
{
    public class JsonPlaceholderRemoteService
    {
        private const string ApiUrl = "https://jsonplaceholder.typicode.com";

        private readonly IRemoteService<IPhotosApiService> _remoteService;
        private readonly ILogger _logger;

        public JsonPlaceholderRemoteService(
            IRemoteServiceFactory remoteServiceFactory,
            IHttpClientFactory httpClientFactory,
            ILogManager logManager)
        {
            _logger = logManager.GetLogger<JsonPlaceholderRemoteService>();

            var httpClient = httpClientFactory.CreateClient(ApiUrl, _logger);

            _remoteService = remoteServiceFactory.Create<IPhotosApiService>(httpClient);
        }

        public async Task<string> GetDataAsync(CancellationToken cancellationToken)
        {
            _logger.Debug("========= Begin =========");

            try
            {
                var result = await _remoteService.MakeRequest(
                    (service, ct) => service.GetAllPhotosAsync(ct),
                    new RequestOptions
                    {
                        RetryCount = 2,
                        Timeout = 2,
                        CancellationToken = cancellationToken
                    }).ConfigureAwait(false);

                return $"Done. Count: {result.Count}";
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            finally
            {
                _logger.Debug("========== End ==========");
            }

            return "Failed. See log.";
        }
    }
}
