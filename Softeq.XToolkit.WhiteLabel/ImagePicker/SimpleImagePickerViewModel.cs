// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common;

namespace Softeq.XToolkit.WhiteLabel.ImagePicker
{
    public class SimpleImagePickerViewModel : ObservableObject
    {
        private string _imageCacheKey;

        public string ImageCacheKey
        {
            get => _imageCacheKey;
            set => Set(ref _imageCacheKey, value);
        }
    }
}
