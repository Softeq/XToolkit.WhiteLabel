
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.Content;
using Plugin.CurrentActivity;
using ImageOrientation = Android.Media.Orientation;
using Debug = System.Diagnostics.Debug;

namespace Softeq.XToolkit.WhiteLabel.Droid.ImagePicker
{
    [Activity(Label = "ImagePickerActivity")]
    internal class ImagePickerActivity : Activity
    {
        public const string ModeKey = "Mode";
        public const int CameraMode = 1;
        public const int GalleryMode = 2;

        private const string ImagesFolder = "Pictures";

        private Android.Net.Uri _fileUri;
        private Intent _pickIntent;

        public static event EventHandler<Bitmap> ImagePicked;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            try
            {
                switch (Intent.GetIntExtra(ModeKey, default(int)))
                {
                    case CameraMode:
                        CaptureCamera();
                        break;
                    case GalleryMode:
                        PickImage();
                        break;
                }
            }
            catch
            {
                OnImagePicked(null);
            }
            finally
            {
                _pickIntent.Dispose();
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            Android.Net.Uri uri = null;
            Bitmap bitmap = null;
            switch (requestCode)
            {
                case CameraMode:
                    uri = _fileUri;
                    break;
                case GalleryMode:
                    uri = data?.Data;
                    break;
            }
            if (uri != null)
            {
                bitmap = MediaStore.Images.Media.GetBitmap(CrossCurrentActivity.Current.AppContext.ContentResolver, uri);
                using (var stream = GetContentSrream(CrossCurrentActivity.Current.AppContext, uri))
                {
                    bitmap = FixRotation(bitmap, new ExifInterface(stream)).Result;
                }
            }
            OnImagePicked(bitmap);
        }

        private void OnImagePicked(Bitmap bitmap)
        {
            ImagePicked?.Invoke(this, bitmap);
            Finish();
        }

        private void CaptureCamera()
        {
            _pickIntent = new Intent(MediaStore.ActionImageCapture);
            _fileUri = GetOutputMediaFile(CrossCurrentActivity.Current.AppContext, ImagesFolder, null);

            _pickIntent.PutExtra(MediaStore.ExtraOutput, _fileUri);
            StartActivityForResult(_pickIntent, CameraMode);
        }

        private void PickImage()
        {
            _pickIntent = new Intent(Intent.ActionPick);
            _pickIntent.SetType("image/*");
            StartActivityForResult(_pickIntent, GalleryMode);
        }

        private Task<Bitmap> FixRotation(Bitmap originalImage, ExifInterface exif)
        {
            return Task.Run(() =>
            {
                var rotation = GetRotation(exif);
                if (rotation == 0)
                {
                    return originalImage;
                }

                var matrix = new Matrix();
                matrix.PostRotate(rotation);
                var rotatedImage = Bitmap.CreateBitmap(originalImage, 0, 0, originalImage.Width, originalImage.Height, matrix, true);
                originalImage.Recycle();
                originalImage.Dispose();
                GC.Collect();
                return rotatedImage;
            });
        }

        private int GetRotation(ExifInterface exif)
        {
            if (exif == null)
                return 0;
            try
            {
                var orientation = (ImageOrientation) exif.GetAttributeInt(ExifInterface.TagOrientation, (int) ImageOrientation.Normal);

                switch (orientation)
                {
                    case ImageOrientation.Rotate90:
                        return 90;
                    case ImageOrientation.Rotate180:
                        return 180;
                    case ImageOrientation.Rotate270:
                        return 270;
                    default:
                        return 0;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#else
                return 0;
#endif
            }
        }

        private System.IO.Stream GetContentSrream(Context context, Android.Net.Uri uri)
        {
            var stream = System.IO.Stream.Null;
            try
            {
                stream = context.ContentResolver.OpenInputStream(uri);
            }
            catch (Java.IO.FileNotFoundException fnfEx)
            {
                Debug.WriteLine("Unable to save picked file from disk " + fnfEx);
            }

            return stream;
        }

        private Android.Net.Uri GetOutputMediaFile(Context context, string subdir, string name)
        {
            subdir = subdir ?? string.Empty;

            if (string.IsNullOrWhiteSpace(name))
            {
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
                name = $"IMG_{timestamp}.jpg";
            }

            var directory = context.CacheDir;
            using (var mediaStorageDir = new Java.IO.File(directory, subdir))
            {
                if (!mediaStorageDir.Exists())
                {
                    if (!mediaStorageDir.Mkdirs())
                    {
                        throw new IOException("Couldn't create directory, have you added the WRITE_EXTERNAL_STORAGE permission?");
                    }

                    using (var nomedia = new Java.IO.File(mediaStorageDir, ".nomedia"))
                    {
                        nomedia.CreateNewFile();
                    }
                }

                return FileProvider.GetUriForFile(context, $"{context.PackageName}.fileprovider", new Java.IO.File(GetUniquePath(mediaStorageDir.Path, name)));
            }
        }

        private static string GetUniquePath(string folder, string name)
        {
            var ext = System.IO.Path.GetExtension(name);
            if (ext == string.Empty)
            {
                ext = ".jpg";
            }

            name = System.IO.Path.GetFileNameWithoutExtension(name);

            var nname = name + ext;
            var i = 1;
            while (File.Exists(System.IO.Path.Combine(folder, nname)))
            {
                nname = name + "_" + (i++) + ext;
            }

            return System.IO.Path.Combine(folder, nname);
        }

    }
}
