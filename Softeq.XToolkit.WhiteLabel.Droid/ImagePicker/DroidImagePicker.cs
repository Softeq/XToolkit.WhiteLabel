using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Provider;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.ImagePicker;
using static Android.Graphics.Bitmap;

namespace Softeq.XToolkit.WhiteLabel.Droid.ImagePicker
{
    public class DroidImagePicker : IImagePicker
    {
        private readonly IPermissionsManager _permissionsManager;

        public DroidImagePicker(IPermissionsManager permissionsManager)
        {
            _permissionsManager = permissionsManager;
        }

        private TaskCompletionSource<Bitmap> _taskCompletionSource;

        public async Task<ImagePickeResult> PickPhotoAsync()
        {
            var permissionStatus = await _permissionsManager.CheckWithRequestAsync<PhotosPermission>().ConfigureAwait(false);

            if(permissionStatus != PermissionStatus.Granted)
            {
                return null;
            }

            return await GetImageAsync(ImagePickerActivity.GalleryMode).ConfigureAwait(false);
        }

        public async Task<ImagePickeResult> TakePhotoAsync()
        {
            var permissionStatus = await _permissionsManager.CheckWithRequestAsync<CameraPermission>().ConfigureAwait(false);

            if (permissionStatus != PermissionStatus.Granted)
            {
                return null;
            }

            return await GetImageAsync(ImagePickerActivity.CameraMode).ConfigureAwait(false);
        }

        private async Task<ImagePickeResult> GetImageAsync(int mode)
        {
            var intent = new Intent(CrossCurrentActivity.Current.Activity, typeof(ImagePickerActivity));

            intent.PutExtra(ImagePickerActivity.ModeKey, mode);

            _taskCompletionSource = new TaskCompletionSource<Bitmap>();

            ImagePickerActivity.ImagePicked += OnImagePicked;

            CrossCurrentActivity.Current.Activity.StartActivity(intent);

            var bitmap = await _taskCompletionSource.Task.ConfigureAwait(false);

            return new DroidImagePickerResult
            {
                ImageObject = bitmap,
                ImageExtension = ImageExtension.Jpg
            };
        }

        private void OnImagePicked(object sender, Bitmap e)
        {
            ImagePickerActivity.ImagePicked -= OnImagePicked;
            _taskCompletionSource.SetResult(e);
        }
    }
}
