// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Net;

namespace Softeq.XToolkit.WhiteLabel.Essentials.Droid.ImagePicker
{
    public interface IImagePickerActivityResultHandler
    {
        Task<Bitmap?> HandleImagePickerCameraResultAsync(Activity activity, Result resultCode, Uri? fileUri);
        
        Task<Bitmap?> HandleImagePickerGalleryResultAsync(Activity activity, Result resultCode, Intent data);
        
        Task HandleCustomResultAsync(int requestCode, Result resultCode, Intent data);
    }
}