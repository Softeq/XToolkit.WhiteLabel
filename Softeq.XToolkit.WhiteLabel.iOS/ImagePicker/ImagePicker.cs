// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using CoreGraphics;
using FFImageLoading;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.ImagePicker;
using Softeq.XToolkit.WhiteLabel.Threading;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.ImagePicker
{
    public class ImagePicker
    {
        private readonly UIImagePickerController _imagePicker;
        private readonly WeakReferenceEx<UIViewController> _viewController;
        private RelayCommand<ImageOpenedEventArgs> _openCommand;
        private IFilesProvider _fileProvider;
        private IPermissionsManager _permissionManager;
        private ImagePickerOptions _options;

        public ImagePicker(UIViewController viewController)
        {
            _viewController = WeakReferenceEx.Create(viewController);
            _fileProvider = WhiteLabelDependencies.InternalStorageProvider;
            _permissionManager = ServiceLocator.Resolve<IPermissionsManager>();

            _imagePicker = new UIImagePickerController();
        }

        public ImagePicker SetOpenAction(RelayCommand<ImageOpenedEventArgs> command)
        {
            _openCommand = command;
            return this;
        }

        public async Task OpenAsync(ImagePickerOptions options)
        {
            _options = options;

            if (!await IsPermissionGranted().ConfigureAwait(false))
            {
                return;
            }

            _imagePicker.SourceType = options.ImagePickerOpenType == ImagePickerOpenTypes.Camera
                ? UIImagePickerControllerSourceType.Camera
                : UIImagePickerControllerSourceType.PhotoLibrary;

            _imagePicker.AllowsEditing = options.AllowEditing;

            _imagePicker.FinishedPickingMedia += OnFinishedPickingMediaAsync;
            _imagePicker.Canceled += OnCanceled;

            _viewController.Target.NavigationController.PresentModalViewController(_imagePicker, true);
        }

        private async Task HandlePickerResultAsync(UIImagePickerMediaPickedEventArgs e)
        {
            string path;
            UIImage thumbnail;

            Unsubscribe();

            var imageKey = _options.AllowEditing ? UIImagePickerController.EditedImage : UIImagePickerController.OriginalImage;
            var image = ToUpImageOrientation((UIImage)e.Info[imageKey]);

            var name = $"{Guid.NewGuid()}.png";

            var stream = await Task.Run(() => image.AsPNG().AsStream()).ConfigureAwait(false);

            path = await _fileProvider.WriteStreamAsync(name, stream).ConfigureAwait(false);

            thumbnail = await ImageService.Instance
                                          .LoadFileFromApplicationBundle(path)
                                          .DownSample(_options.ThumbnailWidth, _options.ThumbnailHeight)
                                          .AsUIImageAsync()
                                          .ConfigureAwait(false);

            Execute.OnUIThread(() =>
            {
                if (_openCommand != null)
                {
                    _openCommand.Execute(new ImageOpenedEventArgs(path, thumbnail));
                }
            });
        }

        public void Unsubscribe()
        {
            Execute.OnUIThread(() =>
            {
                _imagePicker.FinishedPickingMedia -= OnFinishedPickingMediaAsync;
                _imagePicker.Canceled -= OnCanceled;
            });
        }

        private async Task<bool> IsPermissionGranted()
        {
            var permission = _options.ImagePickerOpenType == ImagePickerOpenTypes.Camera
                ? Permission.Camera
                : Permission.Photos;

            return await _permissionManager.CheckWithRequestAsync(permission).ConfigureAwait(false) == PermissionStatus.Granted;
        }

        private void OnCanceled(object sender, EventArgs e)
        {
            Unsubscribe();
            _imagePicker.DismissModalViewController(true);
        }

        private void OnFinishedPickingMediaAsync(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            Execute.OnUIThread(() =>
            {
                _imagePicker.DismissModalViewController(true);
            });

            Task.Run(() => HandlePickerResultAsync(e));
        }

        private UIImage ToUpImageOrientation(UIImage image)
        {
            var orientation = image.Orientation;

            if (orientation == UIImageOrientation.Up)
            {
                return image;
            }

            int degree = 0;
            switch (orientation)
            {
                case UIImageOrientation.Down:
                    degree = 180;
                    break;
                case UIImageOrientation.Left:
                    degree = 270;
                    break;
                case UIImageOrientation.Right:
                    degree = 90;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var rect = orientation == UIImageOrientation.Down
                ? new CGRect(0, 0, image.Size.Width, image.Size.Height)
                : new CGRect(0, 0, image.Size.Height, image.Size.Width);

            UIView view = null;

            Execute.OnUIThreadAsync(() =>
            {
                view = new UIView(rect);
            });

            var radians = degree * (float)Math.PI / 180;
            var transformRotate = CGAffineTransform.MakeRotation(radians);

            var size = new CGSize();

            Execute.OnUIThreadAsync(() =>
            {
                size = view.Frame.Size;
                view.Transform = transformRotate;
            });

            UIGraphics.BeginImageContext(size);
            var context = UIGraphics.GetCurrentContext();

            context.TranslateCTM(size.Width / 2, size.Height / 2);
            context.RotateCTM(radians);
            context.ScaleCTM(1, -1);
            context.DrawImage(new CGRect(-rect.Width / 2, -rect.Height / 2, rect.Width, rect.Height), image.CGImage);

            var imageCopy = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return imageCopy;
        }
    }
}