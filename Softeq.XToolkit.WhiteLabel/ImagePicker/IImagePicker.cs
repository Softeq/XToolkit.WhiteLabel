using System;
using System.Threading.Tasks;

namespace Softeq.XToolkit.WhiteLabel.ImagePicker
{
    public interface IImagePicker
    {
        Task<ImagePickeResult> PickPhotoAsync();

        Task<ImagePickeResult> TakePhotoAsync();
    }
}
