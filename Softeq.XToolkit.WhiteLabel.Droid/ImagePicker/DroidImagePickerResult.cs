using System;
using System.IO;
using System.Threading.Tasks;
using Android.Graphics;
using Softeq.XToolkit.WhiteLabel.ImagePicker;
using static Android.Graphics.Bitmap;

namespace Softeq.XToolkit.WhiteLabel.Droid.ImagePicker
{
    public class DroidImagePickerResult : ImagePickeResult
    {
        public override Task<Stream> GetStream()
        {
            Stream memoryStream = new MemoryStream();
            if(ImageObject != null)
            {
                if(ImageExtension == ImageExtension.Jpg)
                {
                    ((Bitmap) ImageObject).Compress(CompressFormat.Jpeg, 100, memoryStream);
                }
                else if (ImageExtension == ImageExtension.Png)
                {
                    ((Bitmap) ImageObject).Compress(CompressFormat.Png, 100, memoryStream);
                }
            }
            return Task.FromResult(memoryStream);
        }
    }
}
