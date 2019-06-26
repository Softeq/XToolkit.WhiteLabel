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
                var compressFormat = ImageExtension == ImageExtension.Jpg ?
                    CompressFormat.Jpeg : CompressFormat.Png;

                ((Bitmap) ImageObject).Compress(compressFormat, (int) (Quality * 100), memoryStream);
            }
            return Task.FromResult(memoryStream);
        }
    }
}
