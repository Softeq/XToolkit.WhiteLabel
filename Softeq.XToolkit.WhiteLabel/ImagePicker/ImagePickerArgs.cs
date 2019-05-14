using System;
using System.IO;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.WhiteLabel.ImagePicker
{
    public class ImageExtensionToStringConverter : IConverter<string, ImageExtension>
    {
        public string ConvertValue(ImageExtension extension, object parameter = null, string language = null)
        {
            switch (extension)
            {
                case ImageExtension.Png:
                    return ".png";
                case ImageExtension.Jpg:
                    return ".jpg";
                default:
                    throw new ArgumentOutOfRangeException(nameof(extension), extension, null);
            }
        }

        public ImageExtension ConvertValueBack(string value, object parameter = null, string language = null)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value.Contains(".png"))
            {
                return ImageExtension.Png;
            }

            if (value.Contains(".jpg") || value.Contains(".jpeg"))
            {
                return ImageExtension.Jpg;
            }

            return ImageExtension.Unknown;
        }
    }

    public enum ImageExtension
    {
        Unknown,
        Png,
        Jpg
    }

    public class ImagePickerArgs
    {
        public string ImageCacheKey { get; set; }

        public ImageExtension ImageExtension { get; set; }

        public Func<Task<Stream>> ImageStream;

        public string Extension
        {
            get
            {
                var converter = new ImageExtensionToStringConverter();
                return converter.ConvertValue(ImageExtension);
            }
        }
    }
}