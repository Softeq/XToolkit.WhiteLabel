using System;
using System.IO;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.ImagePicker;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.ImagePicker
{
    public class IosImagePickerResult : ImagePickerResult
    {
        public override Task<Stream> GetStream()
        {
            if (ImageObject is UIImage image)
            {
                if(ImageExtension == ImageExtension.Jpg)
                {
                    return Task.FromResult(image.AsJPEG(Quality).AsStream());
                }
                else if (ImageExtension == ImageExtension.Png)
                {
                    return Task.FromResult(image.AsPNG().AsStream());
                }
            }
            return Task.FromResult(Stream.Null);
        }
    }
}
