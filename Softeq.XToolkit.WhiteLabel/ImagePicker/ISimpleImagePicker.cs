// Developed for PAWS-HALO by Softeq Development
// Corporation http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.WhiteLabel.ImagePicker
{
    public interface ISimpleImagePicker
    {
        Task OpenCameraAsync();
        Task OpenGalleryAsync();
        SimpleImagePickerViewModel ViewModel { get; }
    }
}