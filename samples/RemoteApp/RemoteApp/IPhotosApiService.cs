using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Primitives;

namespace RemoteApp
{
    [Headers(
        "User-Agent: " + nameof(RemoteApp),
        Header.Gzip,
        Header.Json)]
    public interface IPhotosApiService
    {
        [Get("/photos")]
        Task<IEnumerable<PhotoDto>> GetAllPhotosAsync(CancellationToken cancellationToken);

        [Get("/photos/{photoId}")]
        Task<PhotoDto> GetPhotoAsync(int photoId, CancellationToken cancellationToken);

        [Get("/photos/{photoId}")]
        [Headers("Authorization: TestScheme")]
        Task<PhotoDto> GetAuthPhotoAsync(int photoId);
    }
}
