// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.WhiteLabel.ImagePicker
{
    /// <summary>
    ///     Service for getting images.
    /// </summary>
    public interface IImagePickerService
    {
        /// <summary>
        ///     Picks a photo from the default gallery.
        /// </summary>
        /// <param name="quality">
        ///     The compression quality to use, 0 is the maximum compression (worse quality),
        ///     and 1 minimum compression (best quality)
        ///     Default is 1 = 100%
        /// </param>
        /// <returns></returns>
        Task<ImagePickerResult?> PickPhotoAsync(float quality = 1);

        /// <summary>
        ///     Take photo from camera.
        /// </summary>
        /// <param name="quality">
        ///     The compression quality to use, 0 is the maximum compression (worse quality),
        ///     and 1 minimum compression (best quality)
        ///     Default is 1 = 100%
        /// </param>
        /// <returns></returns>
        Task<ImagePickerResult?> TakePhotoAsync(float quality = 1);
    }
}
