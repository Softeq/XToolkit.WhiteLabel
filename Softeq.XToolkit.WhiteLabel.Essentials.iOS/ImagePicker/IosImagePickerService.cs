// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using MobileCoreServices;
using Softeq.XToolkit.WhiteLabel.Essentials.ImagePicker;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.Essentials.iOS.ImagePicker
{
    public class IosImagePickerService : IImagePickerService
    {
        private TaskCompletionSource<UIImage?>? _taskCompletionSource;
        private UIImagePickerController? _pickerController;

        /// <inheritdoc />
        public Task<ImagePickerResult?> PickPhotoAsync(float quality)
        {
            return GetImageAsync(UIImagePickerControllerSourceType.PhotoLibrary, quality);
        }

        /// <inheritdoc />
        public Task<ImagePickerResult?> TakePhotoAsync(float quality)
        {
            return GetImageAsync(UIImagePickerControllerSourceType.Camera, quality);
        }

        private async Task<ImagePickerResult?> GetImageAsync(UIImagePickerControllerSourceType type, float quality)
        {
            _pickerController = new UIImagePickerController
            {
                SourceType = type,
                MediaTypes = new string[] { UTType.Image },
            };
            _pickerController.FinishedPickingMedia += OnFinishedPickingMedia;
            _pickerController.Canceled += OnCanceled;

            _taskCompletionSource = new TaskCompletionSource<UIImage?>();

            UIApplication.SharedApplication.KeyWindow.RootViewController!.PresentViewController(_pickerController, true, null);

            var image = await _taskCompletionSource.Task.ConfigureAwait(false);

            return new IosImagePickerResult
            {
                ImageObject = image,
                Quality = quality,
                ImageExtension = ImageExtension.Jpeg
            };
        }

        private void OnFinishedPickingMedia(object? sender, UIImagePickerMediaPickedEventArgs e)
        {
            _pickerController!.FinishedPickingMedia -= OnFinishedPickingMedia;
            _taskCompletionSource!.SetResult(e.OriginalImage);
            ReleaseImagePicker();
        }

        private void OnCanceled(object? sender, EventArgs e)
        {
            _pickerController!.Canceled -= OnCanceled;
            _taskCompletionSource!.SetResult(null);
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
