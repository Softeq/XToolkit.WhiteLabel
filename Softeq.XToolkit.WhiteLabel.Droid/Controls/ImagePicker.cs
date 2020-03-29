// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.IO;
using System.Threading.Tasks;
using FFImageLoading;
using FFImageLoading.Work;
using Plugin.Permissions;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.ImagePicker;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.Droid.Controls
{
    [Obsolete("Use WhiteLabel.ImagePicker.IImagePickerService")]
    public interface IImagePickerService
    {
        Task<string> PickImageAsync();
        Task<string> TakePhotoAsync();
    }

    [Obsolete("Use IImagePickerService")]
    public class ImagePicker
    {
        private const string Png = ".png";
        private readonly IImagePickerService _imagePickerService;

        private readonly IPermissionsManager _permissionsManager;

        public ImagePicker(
            IPermissionsManager permissionsManager,
            IImagePickerService imagePickerService)
        {
            _permissionsManager = permissionsManager;
            _imagePickerService = imagePickerService;
        }

        public SimpleImagePickerViewModel ViewModel { get; } = new SimpleImagePickerViewModel();

        public int MaxImageWidth { get; set; } = 1125;

        public async Task OpenGallery()
        {
            var result = await _permissionsManager.CheckWithRequestAsync<PhotosPermission>().ConfigureAwait(false);
            if (result != PermissionStatus.Granted)
            {
                return;
            }

            var key = await _imagePickerService.PickImageAsync().ConfigureAwait(false);
            Execute.BeginOnUIThread(() => { ViewModel.ImageCacheKey = key; });
        }

        public async Task OpenCamera()
        {
            var result = await _permissionsManager.CheckWithRequestAsync<CameraPermission>().ConfigureAwait(false);
            if (result != PermissionStatus.Granted)
            {
                return;
            }

            result = await _permissionsManager.CheckWithRequestAsync<PhotosPermission>().ConfigureAwait(false);
            if (result != PermissionStatus.Granted)
            {
                return;
            }

            var key = await _imagePickerService.TakePhotoAsync().ConfigureAwait(false);
            Execute.BeginOnUIThread(() => { ViewModel.ImageCacheKey = key; });
        }

        // TODO YP: refactor
        public Func<(Task<Stream?>?, string?)> GetStreamFunc()
        {
            (Task<Stream?>?, string?) getStreamFunc()
            {
                if (ViewModel.ImageCacheKey == null)
                {
                    return (Task.FromResult(default(Stream)), default);
                }

                var imageExtension = GetImageExtension();

                var func = GetLoadTaskFunc(imageExtension);
                return (func == null ? null : func(), GetFileExtension(imageExtension));
            }

            return getStreamFunc;
        }

        public ImagePickerArgs GetPickerData()
        {
            if (string.IsNullOrEmpty(ViewModel.ImageCacheKey))
            {
                return ImagePickerArgs.Empty;
            }

            var imageExtension = GetImageExtension();

            return new ImagePickerArgs
            {
                ImageCacheKey = ViewModel.ImageCacheKey,
                ImageStream = GetLoadTaskFunc(imageExtension),
                ImageExtension = imageExtension
            };
        }

        private Func<Task<Stream?>>? GetLoadTaskFunc(ImageExtension imageExtension)
        {
            switch (imageExtension)
            {
                case ImageExtension.Png:
                    return CreatePngLoadTask;
                case ImageExtension.Jpg:
                    return CreateJpegLoadTask;
                default:
                    return default;
            }
        }

        private string GetFileExtension(ImageExtension imageExtension)
        {
            switch (imageExtension)
            {
                case ImageExtension.Png:
                    return Png;
                default:
                    return Path.GetExtension(ViewModel.ImageCacheKey);
            }
        }

        private ImageExtension GetImageExtension()
        {
            var converter = new ImageExtensionToStringConverter();
            var imageExtension = converter.ConvertValueBack(ViewModel.ImageCacheKey);

            // TODO YP: work with WebP and other as PNG
            if (imageExtension == ImageExtension.Unknown)
            {
                return ImageExtension.Png;
            }

            return imageExtension;
        }

        private async Task<Stream?> CreateJpegLoadTask()
        {
            try
            {
                return await CreateLoadTask().AsJPGStreamAsync(100).ConfigureAwait(false);
            }
            catch (BadImageFormatException ex)
            {
                LogError(ex);
            }

            return default;
        }

        private async Task<Stream?> CreatePngLoadTask()
        {
            try
            {
                return await CreateLoadTask().AsPNGStreamAsync();
            }
            catch (BadImageFormatException ex)
            {
                LogError(ex);
            }

            return default;
        }

        private TaskParameter CreateLoadTask()
        {
            return ImageService.Instance
                .LoadFile(ViewModel.ImageCacheKey)
                .DownSample(MaxImageWidth);
        }

        // TODO YP: refactor, use ILogger from ctor
        private void LogError(BadImageFormatException ex)
        {
            LogManager.LogError<ImagePicker>(ex);
        }
    }
}
