using System;
using System.Threading.Tasks;
using MobileCoreServices;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.ImagePicker;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.ImagePicker
{
    public class IosImagePicker : IImagePicker
    {
        private TaskCompletionSource<UIImage> _taskCompletionSource;
        private UIImagePickerController _pickerController;

        public Task<ImagePickeResult> PickPhotoAsync()
        {
            return GetImageAsync(UIImagePickerControllerSourceType.PhotoLibrary);
        }

        public Task<ImagePickeResult> TakePhotoAsync()
        {
            return GetImageAsync(UIImagePickerControllerSourceType.Camera);
        }

        private async Task<ImagePickeResult> GetImageAsync(UIImagePickerControllerSourceType type)
        {
            _pickerController = new UIImagePickerController
            {
                SourceType = type,
                MediaTypes = new string[] { UTType.Image },
            };
            _pickerController.FinishedPickingMedia += OnFinishedPickingMedia;
            _pickerController.Canceled += OnCanceled;

            _taskCompletionSource = new TaskCompletionSource<UIImage>();

            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(_pickerController, true, null);

            var image = await _taskCompletionSource.Task.ConfigureAwait(false);

            return new IosImagePickerResult
            {
                ImageObject = image,
                ImageExtension = ImageExtension.Jpg
            };
        }

        private void OnFinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            _pickerController.FinishedPickingMedia -= OnFinishedPickingMedia;
            _taskCompletionSource.SetResult(e.OriginalImage);
            ReleaseImagePicker();
        }

        private void OnCanceled(object sender, EventArgs e)
        {
            _pickerController.Canceled -= OnCanceled;
            ReleaseImagePicker();
        }

        private void ReleaseImagePicker()
        {
            if (_pickerController == null)
            {
                return;
            }

            _pickerController.DismissViewController(true, null);
            _pickerController.FinishedPickingMedia -= OnFinishedPickingMedia;
            _pickerController.Canceled -= OnCanceled;
            _pickerController.Dispose();
            _pickerController = null;
        }
    }
}
