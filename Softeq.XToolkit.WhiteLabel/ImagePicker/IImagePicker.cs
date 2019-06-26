using System;
using System.Threading.Tasks;

namespace Softeq.XToolkit.WhiteLabel.ImagePicker
{
    public interface IImagePicker
    {
        Task<ImagePickeResult> PickPhotoAsync(float quality = 1);

        Task<ImagePickeResult> TakePhotoAsync(float quality = 1);
    }
}
