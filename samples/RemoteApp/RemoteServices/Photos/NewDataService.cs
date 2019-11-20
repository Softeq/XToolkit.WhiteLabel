using System;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Primitives;

namespace RemoteServices.Photos
{
    // YP: simple consumer service for make API call as simples as possible.
    // Responsibility:
    // - Fetch data from API;
    // - Catch remote exceptions;
    // - Map data & error DTOs to the app models;
    public class NewDataService
    {
        private readonly IRemoteService<IPhotosApiService> _remoteData;

        public NewDataService(IRemoteServiceFactory remoteServiceFactory)
        {
            _remoteData = remoteServiceFactory.Create<IPhotosApiService>("https://jsonplaceholder.typicode.com");
        }

        public async Task GetAllPhotosAsync(CancellationToken cancellationToken)
        {
            try
            {
                var data = await _remoteData.MakeRequest(
                    (s, ct) => s.GetAllPhotosAsync(ct),
                    new RequestOptions
                    {
                        CancellationToken = cancellationToken
                    });
            }
            catch (ApiException ex)
            {
                // parse to error
            }
            catch (Exception ex)
            {
                // on next
                throw;
            }
        }
    }
}
