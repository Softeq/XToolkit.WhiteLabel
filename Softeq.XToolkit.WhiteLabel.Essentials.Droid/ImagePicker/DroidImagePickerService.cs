// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.Droid.Providers;
using Softeq.XToolkit.WhiteLabel.Essentials.ImagePicker;
using CameraPermission = Microsoft.Maui.ApplicationModel.Permissions.Camera;
using PermissionStatus = Softeq.XToolkit.Permissions.PermissionStatus;
using PhotosPermission = Microsoft.Maui.ApplicationModel.Permissions.Photos;

namespace Softeq.XToolkit.WhiteLabel.Essentials.Droid.ImagePicker
{
    public class DroidImagePickerService : IImagePickerService
    {
        private readonly IPermissionsManager _permissionsManager;
        private readonly IContextProvider _contextProvider;

        private TaskCompletionSource<Bitmap?>? _taskCompletionSource;

        public DroidImagePickerService(
            IPermissionsManager permissionsManager,
            IContextProvider contextProvider)
        {
            _permissionsManager = permissionsManager;
            _contextProvider = contextProvider;
        }

        public async Task<ImagePickerResult?> PickPhotoAsync(float quality)
        {
            var permissionStatus = await _permissionsManager.CheckWithRequestAsync<PhotosPermission>().ConfigureAwait(false);

            if (permissionStatus != PermissionStatus.Granted)
            {
                return null;
            }

            return await GetImageAsync(ImagePickerMode.Gallery, quality).ConfigureAwait(false);
        }

        public async Task<ImagePickerResult?> TakePhotoAsync(float quality)
        {
            var permissionStatus = await _permissionsManager.CheckWithRequestAsync<CameraPermission>().ConfigureAwait(false);

            if (permissionStatus != PermissionStatus.Granted)
            {
                return null;
            }

            return await GetImageAsync(ImagePickerMode.Camera, quality).ConfigureAwait(false);
        }

        private async Task<ImagePickerResult> GetImageAsync(int mode, float quality)
        {
            var activity = _contextProvider.CurrentActivity;
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
                ImageExtension = ImageExtension.Jpeg
            };
        }

        private void OnImagePicked(object? sender, Bitmap? e)
        {
            ImagePickerActivity.ImagePicked -= OnImagePicked;
            _taskCompletionSource!.SetResult(e);
        }
    }
}
