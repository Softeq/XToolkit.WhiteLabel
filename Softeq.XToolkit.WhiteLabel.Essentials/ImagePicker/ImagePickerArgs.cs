// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.IO;
using System.Threading.Tasks;

namespace Softeq.XToolkit.WhiteLabel.Essentials.ImagePicker
{
    [Obsolete("Use IImagePickerService")]
    public class ImagePickerArgs
    {
        public Func<Task<Stream>> ImageStream;
        public string ImageCacheKey { get; set; }

        public ImageExtension ImageExtension { get; set; }

        public string Extension
        {
            get
            {
                var converter = new ImageExtensionToStringConverter();
                return converter.ConvertValue(ImageExtension);
            }
        }

        public static ImagePickerArgs Empty => new ImagePickerArgs
        {
            ImageExtension = ImageExtension.Unknown,
            ImageStream = () => Task.FromResult(default(Stream))
        };
    }
}
