using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace RemoteApp
{
    public interface IApiService
    {
        [Get("/photos")]
        Task<IEnumerable<PhotoDto>> GetAllPhotosAsync();

        [Get("/photos/{photoId}")]
        Task<PhotoDto> GetPhotoAsync(int photoId);

        [Get("/photos/{photoId}")]
        [Headers("Authorization: TestScheme")]
        Task<PhotoDto> GetAuthPhotoAsync(int photoId);
    }
}
