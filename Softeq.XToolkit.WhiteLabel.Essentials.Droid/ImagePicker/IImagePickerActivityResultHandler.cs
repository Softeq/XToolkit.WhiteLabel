// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;

namespace Softeq.XToolkit.WhiteLabel.Essentials.Droid.ImagePicker
{
    public interface IImagePickerActivityResultHandler
    {
        void SetActivity(Activity activity);
        Task<Bitmap?> HandleImagePickerActivityResultAsync(int requestCode, Result resultCode, Intent data, Android.Net.Uri fileUri);
    }
}