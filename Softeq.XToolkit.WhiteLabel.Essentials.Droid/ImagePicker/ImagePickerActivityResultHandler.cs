// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.Net;
using Android.OS;
using Softeq.XToolkit.Common.Weak;

namespace Softeq.XToolkit.WhiteLabel.Essentials.Droid.ImagePicker
{
    public sealed class ImagePickerActivityResultHandler : IImagePickerActivityResultHandler
    {
        private WeakReferenceEx<Activity> _activityRef;

        public void SetActivity(Activity activity)
        {
            _activityRef = new WeakReferenceEx<Activity>(activity);
        }

        public Task<Bitmap?> HandleImagePickerActivityResultAsync(int requestCode, Result resultCode, Intent data, Uri fileUri)
        {
            Bitmap? bitmap = null;

            if (resultCode != Result.Ok)
            {
                return null;
            }

            var uri = requestCode switch
            {
                ImagePickerMode.Camera => fileUri,
                ImagePickerMode.Gallery => data?.Data,
                _ => null
            };

            var context = _activityRef.Target;

            if (uri != null)
            {
                bitmap = ImagePickerUtils.GetBitmap(context, uri);

                if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                {
                    using (var stream = ImagePickerUtils.GetContentStream(context, uri))
                    {
                        bitmap = ImagePickerUtils.FixRotation(bitmap, new ExifInterface(stream)).Result;
                    }
                }
                else
                {
                    bitmap = ImagePickerUtils.FixRotation(bitmap, new ExifInterface(uri.ToString())).Result;
                }
            }

            return Task.FromResult(bitmap);
        }
    }
}