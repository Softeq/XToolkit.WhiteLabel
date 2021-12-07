// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Provider;
using Stream = System.IO.Stream;
using Uri = Android.Net.Uri;

namespace Softeq.XToolkit.WhiteLabel.Essentials.Droid.ImagePicker
{
    public static class ImagePickerUtils
    {
        public static Stream GetContentStream(Context context, Uri uri)
        {
            var stream = Stream.Null;
            try
            {
                stream = context.ContentResolver?.OpenInputStream(uri);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Unable to save picked file from disk: {ex}");
            }

            return stream;
        }

        public static Task<Bitmap?> FixRotation(Bitmap originalImage, ExifInterface exif)
        {
            return Task.Run(() =>
            {
                var rotation = GetRotation(exif);
                if (rotation == 0)
                {
                    return originalImage;
                }

                return RotateImage(originalImage, rotation);
            });
        }

        public static int GetRotation(ExifInterface? exif)
        {
            if (exif == null)
            {
                return 0;
            }

            try
            {
                var orientation = (Orientation) exif.GetAttributeInt(
                    ExifInterface.TagOrientation,
                    (int) Orientation.Normal);

                return orientation switch
                {
                    Orientation.Rotate90 => 90,
                    Orientation.Rotate180 => 180,
                    Orientation.Rotate270 => 270,
                    _ => 0
                };
            }
            catch (Exception)
            {
#if DEBUG
                throw;
#else
                return 0;
#endif
            }
        }

        public static Bitmap? RotateImage(Bitmap originalImage, float degrees)
        {
            var matrix = new Matrix();
            matrix.PostRotate(degrees);
            var rotatedImage = Bitmap.CreateBitmap(originalImage, 0, 0, originalImage.Width, originalImage.Height, matrix, true);
            originalImage.Recycle();
            originalImage.Dispose();
            GC.Collect();
            return rotatedImage;
        }

        public static Bitmap GetBitmap(Context context, Uri uri)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.P)
            {
                return ImageDecoder.DecodeBitmap(ImageDecoder.CreateSource(context.ContentResolver!, uri));
            }

            return MediaStore.Images.Media.GetBitmap(context.ContentResolver, uri);
        }
    }
}
