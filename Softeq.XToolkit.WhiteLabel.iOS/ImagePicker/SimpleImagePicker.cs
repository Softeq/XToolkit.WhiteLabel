// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using FFImageLoading;
using MobileCoreServices;
using Plugin.Permissions;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.iOS.Extensions;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.ImagePicker;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Threading;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.ImagePicker
{
    [Obsolete("Use IImagePickerService")]
    public class SimpleImagePicker : ObservableObject
    {
        private readonly IPermissionsManager _permissionsManager;
        private readonly bool _allowsEditing;
        private readonly WeakReferenceEx<UIViewController> _parentViewControllerRef;

        private UIImagePickerController _imagePicker;
        private Size _calculatedImageSize;
        private UIImage _image;
        private bool _isBusy;

        [Obsolete]
        public event EventHandler PickerWillOpen;

        public SimpleImagePicker(
            UIViewController parentViewController,
            IPermissionsManager permissionsManager,
            bool allowsEditing)
        {
            _parentViewControllerRef = WeakReferenceEx.Create(parentViewController);
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

        public UIImage Image
        {
            get => _image;
            private set
            {
                Set(ref _image, value);
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }

        public int MaxImageWidth { get; set; } = 1125;

        public int MaxImageHeight { get; set; } = 1125;

        public void Clear()
        {
            Image = null;
            ViewModel.ImageCacheKey = null;
            IsBusy = false;
        }

        public async void OpenGalleryAsync()
        {
            var status = await _permissionsManager.CheckWithRequestAsync<PhotosPermission>();
            if (status != PermissionStatus.Granted)
            {
                return;
            }

            _imagePicker = new UIImagePickerController
            {
                SourceType = UIImagePickerControllerSourceType.PhotoLibrary,
                MediaTypes = new string[] { UTType.Image },
                AllowsEditing = _allowsEditing
            };
            OpenSelector();
        }

        public async void OpenCameraAsync()
        {
            var status = await _permissionsManager.CheckWithRequestAsync<CameraPermission>();
            if (status != PermissionStatus.Granted)
            {
                return;
            }

            _imagePicker = new UIImagePickerController
            {
                SourceType = UIImagePickerControllerSourceType.Camera,
                MediaTypes = new string[] { UTType.Image },
                AllowsEditing = _allowsEditing
            };

            OpenSelector();
        }

        public ImagePickerArgs GetPickerData()
        {
            if (string.IsNullOrEmpty(ViewModel.ImageCacheKey))
            {
                return ImagePickerArgs.Empty;
            }

            var imageExtension = ImageExtension.Png;

            if (ViewModel.ImageCacheKey.Contains(".jpg") || ViewModel.ImageCacheKey.Contains(".jpeg"))
            {
                imageExtension = ImageExtension.Jpg;
            }

            _calculatedImageSize = new Size((int) (MaxImageWidth / UIScreen.MainScreen.Scale),
                (int) (MaxImageHeight / UIScreen.MainScreen.Scale));

            var func = default(Func<Task<Stream>>);

            switch (imageExtension)
            {
                case ImageExtension.Png:
                    func = () => ImageService.Instance
                        .LoadFile(ViewModel.ImageCacheKey)
                        .DownSample(_calculatedImageSize.Width, _calculatedImageSize.Height)
                        .AsPNGStreamAsync();
                    break;
                case ImageExtension.Jpg:
                    func = () => ImageService.Instance
                        .LoadFile(ViewModel.ImageCacheKey)
                        .DownSample(_calculatedImageSize.Width, _calculatedImageSize.Height)
                        .AsJPGStreamAsync();
                    break;
            }

            return new ImagePickerArgs
            {
                ImageCacheKey = ViewModel.ImageCacheKey,
                ImageExtension = imageExtension
            };
        }

        private void OpenSelector()
        {
            PickerWillOpen?.Invoke(this, EventArgs.Empty);

            _imagePicker.FinishedPickingMedia += OnFinishedPickingMedia;
            _imagePicker.Canceled += OnCanceled;

            _parentViewControllerRef.Target?.PresentViewController(_imagePicker, true, null);
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
            if (selectedImage == null)
            {
                ReleaseImagePicker();
                return;
            }

            var filePath = GenerateFilePath();

            Execute.BeginOnUIThread(async () =>
            {
                Image = selectedImage.ToUpImageOrientation();
                IsBusy = true;
                ReleaseImagePicker();

                if (args.ImageUrl == null)
                {
                    await SaveImage(Image, filePath);
                    ViewModel.ImageCacheKey = filePath;
                }
                else
                {
                    ViewModel.ImageCacheKey = args.ImageUrl.Path;
                }
                IsBusy = false;
            });
        }

        private string GenerateFilePath()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Guid.NewGuid().ToString());
        }

        private Task SaveImage(UIImage image, string filePath)
        {
            return Task.Run(() => Image.AsPNG().Save(filePath, true));
        }

        private void ReleaseImagePicker()
        {
            if (_imagePicker == null)
            {
                return;
            }

            _imagePicker.DismissViewController(true, null);
            _imagePicker.FinishedPickingMedia -= OnFinishedPickingMedia;
            _imagePicker.Canceled -= OnCanceled;
            _imagePicker.Dispose();
            _imagePicker = null;
        }
    }
}
