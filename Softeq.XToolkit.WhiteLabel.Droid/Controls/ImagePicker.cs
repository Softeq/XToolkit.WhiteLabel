// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.IO;
using System.Threading.Tasks;
using FFImageLoading;
using FFImageLoading.Work;
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
        private const string Png = ".png";

        private readonly IPermissionsManager _permissionsManager;
        private readonly IImagePickerService _imagePickerService;

        public ImagePicker(
            IPermissionsManager permissionsManager,
            IImagePickerService imagePickerService)
        {
            _permissionsManager = permissionsManager;
            _imagePickerService = imagePickerService;
        }

        public SimpleImagePickerViewModel ViewModel { get; } = new SimpleImagePickerViewModel();

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

            result = await _permissionsManager.CheckWithRequestAsync(Permission.Photos).ConfigureAwait(false);
            if (result != PermissionStatus.Granted)
            {
                return;
            }

            var key = await _imagePickerService.TakePhotoAsync().ConfigureAwait(false);
            Execute.BeginOnUIThread(() => { ViewModel.ImageCacheKey = key; });
        }

        // TODO YP: refactor
        public Func<(Task<Stream>, string)> GetStreamFunc()
        {
            (Task<Stream>, string) getStreamFunc()
            {
                if (ViewModel.ImageCacheKey == null)
                {
                    return (Task.FromResult(default(Stream)), default(string));
                }

                var imageExtension = GetImageExtension();

                return (GetLoadTaskFunc(imageExtension)(), GetFileExtension(imageExtension));
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

        private Func<Task<Stream>> GetLoadTaskFunc(ImageExtension imageExtension)
        {
            switch (imageExtension)
            {
                case ImageExtension.Png:
                    return CreatePngLoadTask;
                case ImageExtension.Jpg:
                    return CreateJpegLoadTask;
                default:
                    return default(Func<Task<Stream>>);
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

        private async Task<Stream> CreateJpegLoadTask()
        {
            try
            {
                return await CreateLoadTask().AsJPGStreamAsync(100).ConfigureAwait(false);
            }
            catch (BadImageFormatException ex)
            {
                LogError(ex);
            }
            return default(Stream);
        }

        private async Task<Stream> CreatePngLoadTask()
        {
            try
            {
                return await CreateLoadTask().AsPNGStreamAsync();
            }
            catch (BadImageFormatException ex)
            {
                LogError(ex);
            }
            return default(Stream);
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