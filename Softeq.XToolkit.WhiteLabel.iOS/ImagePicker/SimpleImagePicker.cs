// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using FFImageLoading;
using MobileCoreServices;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.iOS.Extensions;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.ImagePicker;
using Softeq.XToolkit.WhiteLabel.Threading;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.ImagePicker
{
    public class SimpleImagePicker
    {
        private readonly IPermissionsManager _permissionsManager;
        private readonly bool _allowsEditing;
        private readonly WeakReferenceEx<UIViewController> _parentViewControllerRef;
        
        private UIImagePickerController _imagePicker;
        private Size _calculatedImageSize;

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

        public int MaxImageWidth { get; set; } = 1125;
        
        public int MaxImageHeight { get; set; } = 1125;

        public async void OpenGalleryAsync()
        {
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

        public async void OpenCameraAsync()
        {
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

            selectedImage = selectedImage.ToUpImageOrientation();

            Execute.BeginOnUIThread(() =>
            {
                ReleaseImagePicker();
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

        private void ReleaseImagePicker()
        {
            _imagePicker.DismissViewController(true, null);
            _imagePicker.FinishedPickingMedia -= OnFinishedPickingMedia;
            _imagePicker.Canceled -= OnCanceled;
            _imagePicker.Dispose();
            _imagePicker = null;
        }
    }
}