// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Essentials.ImagePicker;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.Essentials.iOS.ImagePicker
{
    public class IosImagePickerResult : ImagePickerResult
    {
        public override Task<Stream> GetStream()
        {
            if (ImageObject is UIImage image)
            {
                switch (ImageExtension)
                {
                    case ImageExtension.Jpg:
                    case ImageExtension.Jpeg:
                        return Task.FromResult(image.AsJPEG(Quality).AsStream());
                    case ImageExtension.Png:
                        return Task.FromResult(image.AsPNG().AsStream());
                }
            }

            return Task.FromResult(Stream.Null);
        }
    }
}
