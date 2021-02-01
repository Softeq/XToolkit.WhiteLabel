// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Converters;

namespace Softeq.XToolkit.WhiteLabel.Essentials.ImagePicker
{
    public class ImageExtensionToStringConverter : IConverter<string?, ImageExtension>
    {
        public string? ConvertValue(ImageExtension extension, object? parameter = null, string? language = null)
        {
            return extension switch
            {
                ImageExtension.Png => ".png",
                ImageExtension.Jpg => ".jpg",
                ImageExtension.Jpeg => ".jpeg",
                ImageExtension.Unknown => null,
                _ => throw new ArgumentOutOfRangeException(nameof(extension), extension, null)
            };
        }

        public ImageExtension ConvertValueBack(string? value, object? parameter = null, string? language = null)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value.Contains(".png"))
            {
                return ImageExtension.Png;
            }

            if (value.Contains(".jpg"))
            {
                return ImageExtension.Jpg;
            }

            if (value.Contains(".jpeg"))
            {
                return ImageExtension.Jpeg;
            }

            return ImageExtension.Unknown;
        }
    }
}
