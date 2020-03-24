// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Plugin.Permissions;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.ImagePicker;

namespace Softeq.XToolkit.WhiteLabel.Droid.ImagePicker
{
    public class DroidImagePickerService : IImagePickerService
    {
        private readonly IPermissionsManager _permissionsManager;

        private TaskCompletionSource<Bitmap?>? _taskCompletionSource;

        public DroidImagePickerService(IPermissionsManager permissionsManager)
        {
            _permissionsManager = permissionsManager;
        }

        public async Task<ImagePickerResult?> PickPhotoAsync(float quality)
        {
            var permissionStatus = await _permissionsManager.CheckWithRequestAsync<PhotosPermission>().ConfigureAwait(false);

            if (permissionStatus != PermissionStatus.Granted)
            {
                return null;
            }

            return await GetImageAsync(ImagePickerActivity.GalleryMode, quality).ConfigureAwait(false);
        }

        public async Task<ImagePickerResult?> TakePhotoAsync(float quality)
        {
            var permissionStatus = await _permissionsManager.CheckWithRequestAsync<CameraPermission>().ConfigureAwait(false);

            if (permissionStatus != PermissionStatus.Granted)
            {
                return null;
            }

            return await GetImageAsync(ImagePickerActivity.CameraMode, quality).ConfigureAwait(false);
        }

        private async Task<ImagePickerResult> GetImageAsync(int mode, float quality)
        {
            var activity = MainApplicationBase.CurrentActivity;
            var intent = new Intent(activity, typeof(ImagePickerActivity));

            intent.PutExtra(ImagePickerActivity.ModeKey, mode);

            _taskCompletionSource = new TaskCompletionSource<Bitmap?>();

            ImagePickerActivity.ImagePicked += OnImagePicked;

            activity.StartActivity(intent);

            var bitmap = await _taskCompletionSource.Task.ConfigureAwait(false);

            return new DroidImagePickerResult
            {
                Quality = quality,
                ImageObject = bitmap,
                ImageExtension = ImageExtension.Jpg
            };
        }

        private void OnImagePicked(object sender, Bitmap? e)
        {
            ImagePickerActivity.ImagePicked -= OnImagePicked;
            _taskCompletionSource!.SetResult(e);
        }
    }
}
