// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Playground.RemoteData.Test.Dtos;
using Refit;
using Softeq.XToolkit.Remote.Primitives;

namespace Playground.RemoteData.Test
{
    [Headers(
        "User-Agent: XToolkit.Remote",
        Header.Gzip,
        Header.Json)]
    public interface IPhotosApiService
    {
        [Get("/photos")]
        Task<IReadOnlyCollection<PhotoResponse>> GetAllPhotosAsync(CancellationToken cancellationToken);

        [Get("/photos/{photoId}")]
        Task<PhotoResponse> GetPhotoAsync(int photoId, CancellationToken cancellationToken);

        [Get("/photos/{photoId}")]
        [Headers("Authorization: TestScheme")]
        Task<PhotoResponse> GetAuthPhotoAsync(int photoId);
    }
}
