// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Net;
using AndroidX.Activity.Result;
using AndroidX.Activity.Result.Contract;
using AndroidX.AppCompat.App;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.Droid.Providers;
using Softeq.XToolkit.WhiteLabel.Essentials.ImagePicker;
using CameraPermission = Microsoft.Maui.ApplicationModel.Permissions.Camera;
using PermissionStatus = Softeq.XToolkit.Permissions.PermissionStatus;

namespace Softeq.XToolkit.WhiteLabel.Essentials.Droid.ImagePicker
{
    public class DroidImagePickerService : Java.Lang.Object, IImagePickerService, IActivityResultCallback
    {
        private readonly IPermissionsManager _permissionsManager;
        private readonly IContextProvider _contextProvider;
        private readonly ActivityResultLauncher? _activityResultLauncher;

        private TaskCompletionSource<Uri?>? _pickPhotoFileUriCompletionSource;
        private TaskCompletionSource<Bitmap?>? _bitmapTaskCompletionSource;

        public DroidImagePickerService(
            IPermissionsManager permissionsManager,
            IContextProvider contextProvider)
        {
            _permissionsManager = permissionsManager;
            _contextProvider = contextProvider;
            if (ActivityResultContracts.PickVisualMedia.InvokeIsPhotoPickerAvailable(_contextProvider.CurrentActivity)
                && _contextProvider.CurrentActivity is AppCompatActivity appCompatActivity)
            {
                _activityResultLauncher = appCompatActivity.RegisterForActivityResult(
                    new ActivityResultContracts.PickVisualMedia(), this);
            }
        }

        public async Task<ImagePickerResult?> PickPhotoAsync(float quality)
        {
            if (_activityResultLauncher != null)
            {
                _pickPhotoFileUriCompletionSource = new TaskCompletionSource<Uri?>();

                var request = new PickVisualMediaRequest.Builder()
                    .SetMediaType(ActivityResultContracts.PickVisualMedia.ImageOnly.Instance)
                    .Build();
                _activityResultLauncher.Launch(request);

                var fileUri = await _pickPhotoFileUriCompletionSource.Task
                    .ConfigureAwait(false);

                return await GetImageAsync(ImagePickerMode.ImageCropOnly, quality, fileUri)
                    .ConfigureAwait(false);
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

        private async Task<ImagePickerResult> GetImageAsync(int mode, float quality, Uri? fileUri = null)
        {
            var activity = _contextProvider.CurrentActivity;
            var intent = new Intent(activity, typeof(ImagePickerActivity));

            intent.PutExtra(ImagePickerActivity.ModeKey, mode);

            if (mode == ImagePickerMode.ImageCropOnly)
            {
                intent.SetData(fileUri);
            }

            _bitmapTaskCompletionSource = new TaskCompletionSource<Bitmap?>();

            ImagePickerActivity.ImagePicked += OnImagePicked;

            activity.StartActivity(intent);

            var bitmap = await _bitmapTaskCompletionSource.Task.ConfigureAwait(false);

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
            _bitmapTaskCompletionSource!.SetResult(e);
        }

        void IActivityResultCallback.OnActivityResult(Java.Lang.Object? result)
        {
            var uri = result as Uri;
            _pickPhotoFileUriCompletionSource!.TrySetResult(uri);
        }
    }
}
