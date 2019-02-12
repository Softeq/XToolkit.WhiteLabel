// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.ImagePicker
{
    public class SimpleImagePickerViewModel : ObservableObject
    {
        private string _imageCacheKey;
        private bool _isImageProcessing;
        private Stream _imageStream;

        public string ImageCacheKey
        {
            get => _imageCacheKey;
            set => Set(ref _imageCacheKey, value);
        }

        public bool IsImageProcessing
        {
            get => _isImageProcessing;
            set => Set(ref _isImageProcessing, value);
        }

        public Stream ImageStream
        {
            get => _imageStream;
            set => Set(ref _imageStream, value);
        }
    }
}