// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using FFImageLoading;
using MobileCoreServices;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.ImagePicker;
using Softeq.XToolkit.WhiteLabel.Threading;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.ImagePicker
{
    public class SimpleImagePicker : ISimpleImagePicker
    {
        private readonly IPermissionsManager _permissionsManager;
        private readonly bool _allowsEditing;

        private UIImagePickerController _imagePicker;
        private Size _calculatedImageSize;
        private ICropper _cropper;
        private UIViewController _lastUsedViewController;
        private ImagePickerOpenTypes _lastOpenedType;

        public event EventHandler PickerWillOpen;

        public SimpleImagePicker(IPermissionsManager permissionsManager, bool allowsEditing)
        {
            _permissionsManager = permissionsManager;
            _allowsEditing = allowsEditing;

            ViewModel = new SimpleImagePickerViewModel();
        }

        public SimpleImagePickerViewModel ViewModel { get; }

        public Func<(Task<Stream>, string)> StreamFunc
        {
            get
            {
                _calculatedImageSize = new Size((int) (MaxImageWidth / UIScreen.MainScreen.Scale),
                    (int) (MaxImageHeight / UIScreen.MainScreen.Scale));

                Func<(Task<Stream>, string)> func = () =>
                {
                    if (ViewModel.ImageCacheKey == null)
                    {
                        return (Task.FromResult(default(Stream)), default(string));
                    }

                    return (ImageService.Instance
                        .LoadFile(ViewModel.ImageCacheKey)
                        .DownSample(_calculatedImageSize.Width, _calculatedImageSize.Height)
                        .AsPNGStreamAsync(), ".png");
                };

                return func;
            }
        }

        public int MaxImageWidth { get; set; } = 1125;

        public int MaxImageHeight { get; set; } = 1125;

        public SimpleImagePicker SetCropper(ICropper cropper)
        {
            _cropper = cropper;
            return this;
        }

        public async Task OpenGalleryAsync()
        {
            _lastOpenedType = ImagePickerOpenTypes.Gallery;

            var status = await _permissionsManager.CheckWithRequestAsync(Permission.Photos);
            if (status != PermissionStatus.Granted)
            {
                return;
            }

            _imagePicker = new UIImagePickerController
            {
                SourceType = UIImagePickerControllerSourceType.PhotoLibrary,
                MediaTypes = new string[] {UTType.Image},
                AllowsEditing = _allowsEditing
            };
            OpenSelector();
        }

        public async Task OpenCameraAsync()
        {
            _lastOpenedType = ImagePickerOpenTypes.Camera;

            var status = await _permissionsManager.CheckWithRequestAsync(Permission.Camera);
            if (status != PermissionStatus.Granted)
            {
                return;
            }

            _imagePicker = new UIImagePickerController
            {
                SourceType = UIImagePickerControllerSourceType.Camera,
                MediaTypes = new string[] {UTType.Image},
                AllowsEditing = _allowsEditing
            };

            OpenSelector();
        }

        private void OpenSelector()
        {
            PickerWillOpen?.Invoke(this, EventArgs.Empty);

            _imagePicker.FinishedPickingMedia += OnFinishedPickingMedia;
            _imagePicker.Canceled += OnCanceled;

            _lastUsedViewController = Dependencies.IocContainer.Resolve<IViewLocator>().GetTopViewController();
            _lastUsedViewController.PresentViewController(_imagePicker, true, null);
        }

        private void OnFinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            OnImageSelected(e);
        }

        private void OnCanceled(object sender, EventArgs e)
        {
            ReleaseImagePicker();
        }

        private void OnImageSelected(UIImagePickerMediaPickedEventArgs args)
        {
            var selectedImage = _allowsEditing ? args.EditedImage : args.OriginalImage;

            //if picker was dismissed
            if (selectedImage == null)
            {
                ReleaseImagePicker();
            }
            //if not dismissed and has cropper, apply it
            else if (_cropper != null)
            {
                ReleaseImagePicker(() =>
                {
                    _cropper.FinishedCropping += OnCroppingEnded;
                    _cropper.StartCropping(selectedImage, _lastUsedViewController);
                });
            }
            //just save image if don't has cropper
            else
            {
                ReleaseImagePicker();
                SaveImage(selectedImage);
            }
        }

        private async void OnCroppingEnded(object sender, CropEventArgs e)
        {
            _cropper.FinishedCropping -= OnCroppingEnded;

            if (!e.IsDismissed)
            {
                SaveImage(e.Image);
                return;
            }

            switch (_lastOpenedType)
            {
                case ImagePickerOpenTypes.Camera:
                    await OpenCameraAsync().ConfigureAwait(false);
                    break;
                case ImagePickerOpenTypes.Gallery:
                    await OpenGalleryAsync().ConfigureAwait(false);
                    break;
            }
        }

        private void SaveImage(UIImage selectedImage)
        {
            Execute.BeginOnUIThread(() =>
            {
                Task.Run(async () =>
                {
                    using (var stream = selectedImage.AsPNG().AsStream())
                    {
                        var filePath = Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                            Guid.NewGuid().ToString());

                        using (var outputStream = File.OpenWrite(filePath))
                        {
                            var buffer = new byte[4096];
                            int bytesRead;
                            while ((bytesRead = await stream.ReadAsync(
                                       buffer, 0, 4096).ConfigureAwait(false)) > 0)
                            {
                                await outputStream.WriteAsync(buffer, 0, bytesRead).ConfigureAwait(false);
                            }
                        }

                        ViewModel.ImageCacheKey = filePath;
                    }
                });
            });
        }

        private void ReleaseImagePicker(Action action = null)
        {
            _imagePicker.DismissViewController(true, action);
            _imagePicker.FinishedPickingMedia -= OnFinishedPickingMedia;
            _imagePicker.Canceled -= OnCanceled;
            _imagePicker.Dispose();
            _imagePicker = null;
            _lastUsedViewController = null;
        }
    }
}