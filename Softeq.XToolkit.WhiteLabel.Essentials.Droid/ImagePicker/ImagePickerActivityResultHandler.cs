// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.Net;
using Android.OS;

namespace Softeq.XToolkit.WhiteLabel.Essentials.Droid.ImagePicker
{
    public sealed class ImagePickerActivityResultHandler : IImagePickerActivityResultHandler
    {
        public Task<Bitmap?> HandleImagePickerCameraResultAsync(Activity activity, Result resultCode, Uri? fileUri)
        {
            if (resultCode == Result.Ok && fileUri != null)
            {
                return GetBitmapFromUriAsync(activity, fileUri);
            }

            return Task.FromResult(default(Bitmap?));
        }

        public Task<Bitmap?> HandleImagePickerGalleryResultAsync(Activity activity, Result resultCode, Intent data)
        {
            var fileUri = data?.Data;

            if (resultCode == Result.Ok && fileUri != null)
            {
                return GetBitmapFromUriAsync(activity, fileUri);
            }

            return Task.FromResult(default(Bitmap?));
        }

        public Task HandleCustomResultAsync(int requestCode, Result resultCode, Intent data)
        {
            throw new System.NotImplementedException();
        }

        private async Task<Bitmap> GetBitmapFromUriAsync(Context context, Uri fileUri)
        {
            var bitmap = ImagePickerUtils.GetBitmap(context, fileUri);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
            {
                using (var stream = ImagePickerUtils.GetContentStream(context, fileUri))
                {
                    bitmap = await ImagePickerUtils.FixRotation(bitmap, new ExifInterface(stream))
                        .ConfigureAwait(false);
                }
            }
            else
            {
                bitmap = await ImagePickerUtils.FixRotation(bitmap, new ExifInterface(fileUri.ToString()))
                    .ConfigureAwait(false);
            }

            return bitmap;
        }
    }
}