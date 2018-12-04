// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.IO;
using System.Threading.Tasks;
using FFImageLoading;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.ImagePicker;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.Droid.Controls
{
    public interface IImagePickerService
    {
        Task<string> PickImageAsync();
        Task<string> TakePhotoAsync();
    }

    public class ImagePicker
    {
        private readonly IPermissionsManager _permissionsManager;
        private readonly IImagePickerService _imagePickerService;

        public ImagePicker(
            IPermissionsManager permissionsManager, 
            IImagePickerService imagePickerService)
        {
            _permissionsManager = permissionsManager;
            _imagePickerService = imagePickerService;
            ViewModel = new SimpleImagePickerViewModel();
        }

        public SimpleImagePickerViewModel ViewModel { get; }

        public int MaxImageWidth { get; set; } = 1125;

        public async void OpenGallery()
        {
            var result = await _permissionsManager.CheckWithRequestAsync(Permission.Photos).ConfigureAwait(false);
            if (result != PermissionStatus.Granted)
            {
                return;
            }

            var key = await _imagePickerService.PickImageAsync().ConfigureAwait(false);
            Execute.BeginOnUIThread(() => { ViewModel.ImageCacheKey = key; });
        }

        public async void OpenCamera()
        {
            var result = await _permissionsManager.CheckWithRequestAsync(Permission.Camera).ConfigureAwait(false);
            if (result != PermissionStatus.Granted)
            {
                return;
            }

            var key = await _imagePickerService.TakePhotoAsync().ConfigureAwait(false);
            Execute.BeginOnUIThread(() => { ViewModel.ImageCacheKey = key; });
        }

        public Func<(Task<Stream>, string)> GetStreamFunc()
        {
            Func<(Task<Stream>, string)> getStreamFunc = () =>
            {
                if (ViewModel.ImageCacheKey == null)
                {
                    return (Task.FromResult(default(Stream)), default(string));
                }

                var isPng = ViewModel.ImageCacheKey.Contains(".png");
                var expr = ImageService.Instance
                    .LoadFile(ViewModel.ImageCacheKey)
                    .DownSample(MaxImageWidth, 0);

                return isPng
                    ? (expr.AsPNGStreamAsync(), ".png")
                    : (expr.AsJPGStreamAsync(), Path.GetExtension(ViewModel.ImageCacheKey));
            };

            return getStreamFunc;
        }
    }
}