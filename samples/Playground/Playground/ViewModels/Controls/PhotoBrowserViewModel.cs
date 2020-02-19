// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Essentials.ImagePicker;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Playground.ViewModels.Controls
{
    public class PhotoBrowserViewModel : ViewModelBase
    {
        private readonly IImagePickerService _imagePickerService;

        private IDisposable _image;

        public PhotoBrowserViewModel(IImagePickerService imagePickerService)
        {
            _imagePickerService = imagePickerService;

            GalleryCommand = new AsyncCommand(OnGallery);
            CameraCommand = new AsyncCommand(OnCamera);
        }

        public ICommand GalleryCommand { get; }

        public ICommand CameraCommand { get; }

        public IDisposable Image
        {
            get => _image;
            set => Set(ref _image, value);
        }

        private async Task OnGallery()
        {
            var result = await _imagePickerService.PickPhotoAsync().ConfigureAwait(false);

            Execute.BeginOnUIThread(() =>
            {
                Image = result.ImageObject;
            });
        }

        private async Task OnCamera()
        {
            var result = await _imagePickerService.TakePhotoAsync().ConfigureAwait(false);

            Execute.BeginOnUIThread(() =>
            {
                Image = result.ImageObject;
            });
        }
    }
}
