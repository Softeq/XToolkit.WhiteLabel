using System;
using System.IO;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.ImagePicker;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.ImagePicker
{
    public class IosImagePickerResult : ImagePickeResult
    {
        public override Task<Stream> GetStream()
        {
            if(ImageObject != null)
            {
                if(ImageExtension == ImageExtension.Jpg)
                {
                    return Task.FromResult(((UIImage) ImageObject).AsJPEG(Quality).AsStream());
                }
                else if (ImageExtension == ImageExtension.Png)
                {
                    return Task.FromResult(((UIImage) ImageObject).AsPNG().AsStream());
                }
            }
            return Task.FromResult(Stream.Null);
        }
    }
}
