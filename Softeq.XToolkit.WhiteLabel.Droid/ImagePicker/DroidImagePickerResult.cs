// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;
using System.Threading.Tasks;
using Android.Graphics;
using Softeq.XToolkit.WhiteLabel.ImagePicker;
using static Android.Graphics.Bitmap;

namespace Softeq.XToolkit.WhiteLabel.Droid.ImagePicker
{
    public class DroidImagePickerResult : ImagePickerResult
    {
        public override Task<Stream> GetStream()
        {
            Stream memoryStream = new MemoryStream();
            if (ImageObject is Bitmap bitmap)
            {
                var compressFormat = ImageExtension == ImageExtension.Jpg
                    ? CompressFormat.Jpeg
                    : CompressFormat.Png;

                bitmap.Compress(compressFormat, (int) (Quality * 100), memoryStream);
            }

            return Task.FromResult(memoryStream);
        }
    }
}
