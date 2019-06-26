using System;
using System.Threading.Tasks;

namespace Softeq.XToolkit.WhiteLabel.ImagePicker
{
    public interface IImagePickerService
    {
        Task<ImagePickerResult> PickPhotoAsync(float quality = 1);

        Task<ImagePickerResult> TakePhotoAsync(float quality = 1);
    }
}
