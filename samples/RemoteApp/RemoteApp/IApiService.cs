using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace RemoteApp
{
    public interface IApiService
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
